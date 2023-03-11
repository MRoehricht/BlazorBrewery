using BlazorBrewery.Core.Common;
using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Core.Services;
using BlazorBreweryInterface.Controller;
using BlazorBreweryInterface.Fake.Controller;
using BlazorBreweryInterface.Interfaces;
using Microsoft.Extensions.Logging;

namespace BlazorBrewery.BrewComputer.Manager
{
    public class TemperatureManager : ITemperatureManager, IRelayManagerConsumer
    {
        private readonly IThermometerController _thermometerController;
        private readonly IPinController _heatingPinController;
        private readonly IRelayManager _relayManager;
        private readonly ILogger<TemperatureManager> _logger;
        private readonly int _heatPinId = -1;

        private ManagerMode _managerMode = ManagerMode.Auto;
        private double _targetTemperature;
        private BrewingStepTyp _brewingStepTyp;
        private int _durationMinutes;
        private bool _isRunning = false;

        public ManagerMode ManagerMode
        {
            get
            {
                return _managerMode;
            }
            set
            {
                _managerMode = value;
                SwitchMode(value);
            }
        }

        public int PinId => _heatPinId;

        public Action? WorkDone { get; set; }
        private Timer? _stopTimer = null;
        private Timer? _tempCheckTimer = null;
        private IProgress<int>? _progress = null;
        private DateTime? _startTime;
        private double? _startTemperature;

        Action TempHasChanged { get; set; }

        public TemperatureManager(IThermometerController thermometerController, IRelayManager relayManager, ILogger<TemperatureManager> logger, IConfigurationStoreService configurationStoreService)
        {
            _thermometerController = thermometerController;

            _relayManager = relayManager;
            _logger = logger;

            if (!_thermometerController.IsThermometerDeviceAvailable())
            {
                throw new ApplicationException("ThermometerDevice is not available!");
            }

            _heatPinId = configurationStoreService.HeatPinId;
            if (_heatPinId != -1)
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                _heatingPinController = (env != null && env == "Development") ? new FakePinController(_heatPinId) : new PinController(_heatPinId);

            }
            else
            {
                throw new ApplicationException("HeatPinId ist auf -1 gestellt. Bitte Konfiguration prüfen.");
            }

            relayManager.Register(this);
        }

        public async Task Work(double targetTemperature, int durationMinutes, BrewingStepTyp brewingStepTyp, IProgress<int> progress)
        {
            if (_heatPinId == -1 || brewingStepTyp == BrewingStepTyp.Manually) return;
            _targetTemperature = targetTemperature;
            _brewingStepTyp = brewingStepTyp;
            _durationMinutes = durationMinutes;
            _isRunning = true;
            _progress = progress;
            _startTime = DateTime.Now;
            _startTemperature = await GetCurrentTemperature();

            if (_durationMinutes > 0)
            {
                var runMilliSekonds = _durationMinutes * 60000;

                _stopTimer = new Timer(
               callback: new TimerCallback(StopWorkingCallback),
               state: null,
               dueTime: runMilliSekonds,
               period: 0);
            }

            StartTimer();
        }

        private void StartTimer()
        {
            _tempCheckTimer = new Timer(
               callback: new TimerCallback(CheckTemperatureCallback),
               state: null,
               dueTime: 2000,
               period: 1000);
        }

        private void CheckTemperatureCallback(object? state)
        {
            if (ManagerMode != ManagerMode.Auto) return;
            _ = CheckTemperature();
        }

        private void StopWorkingCallback(object? state)
        {
            _tempCheckTimer?.Dispose();
            TurnOff();
            WorkDone?.Invoke();
            _progress?.Report(100);
        }

        private void Calculate(double currentTemperature)
        {
            if (_brewingStepTyp == BrewingStepTyp.Heat && _startTemperature.HasValue)
            {
                var fullDif = _targetTemperature - _startTemperature.Value;
                var targetDif = currentTemperature - _startTemperature.Value;
                var progressValue = (int)((targetDif * 100) / fullDif);
                Console.WriteLine(progressValue);
                _progress?.Report(progressValue);

            }
            else if (_brewingStepTyp == BrewingStepTyp.HoldTemperature && _startTime.HasValue)
            {
                var currentSpam = DateTime.Now - _startTime.Value;
                var progressValue = (int)((currentSpam.TotalSeconds * 100) / (_durationMinutes * 60));
                Console.WriteLine(progressValue);
                _progress?.Report(progressValue);
            }
        }

        public async Task CheckTemperature()
        {
            if (ManagerMode != ManagerMode.Auto) return;
            var currentTemperature = await GetCurrentTemperature();
            Calculate(currentTemperature);
            if (_brewingStepTyp == BrewingStepTyp.Heat)
            {
                if (currentTemperature < (_targetTemperature + 1))
                {
                    TurnOn();
                }
                else
                {
                    StopWorkingCallback(null);
                }
            }
            else if (_brewingStepTyp == BrewingStepTyp.HoldTemperature)
            {
                if (_heatingPinController.IsOn)
                {
                    if (currentTemperature >= _targetTemperature + 1)
                    {
                        TurnOff();
                    }
                }
                else
                {
                    if (currentTemperature <= _targetTemperature - 1)
                    {
                        TurnOn();
                    }
                }
            }
        }

        public async Task<double> GetCurrentTemperature()
        {
            var currentTemperature = await _thermometerController.ReadTemperature();
            return currentTemperature;
        }

        private void SwitchMode(ManagerMode mode)
        {
            if (_heatPinId == -1) return;

            if (mode == ManagerMode.On)
            {
                _tempCheckTimer?.Dispose();
                TurnOn();

            }
            else if (mode == ManagerMode.Off)
            {
                _tempCheckTimer?.Dispose();
                TurnOff();

            }
            else if (mode == ManagerMode.Auto)
            {
                if (_isRunning)
                {
                    CheckTemperatureCallback(null);
                    StartTimer();
                }
                else
                {
                    TurnOff();
                }
            }
        }

        private void TurnOn()
        {
            if (_heatingPinController.IsOn) return;
            _heatingPinController.Shift(Constants.ON);
            _relayManager.SetPinState(_heatPinId, Constants.ON);
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - Heizung AN");
        }

        private void TurnOff()
        {
            if (!_heatingPinController.IsOn) return;
            _heatingPinController.Shift(Constants.OFF);
            _relayManager.SetPinState(_heatPinId, Constants.OFF);
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - Heizung AUS");
        }

        public void StateChanged(int pinId, bool state)
        {

        }

        public void ModeChanged(int pinId, ManagerMode managerMode)
        {
            if (pinId != _heatPinId || pinId == -1) return;
            ManagerMode = managerMode;
        }

        public void StopWork()
        {
            _isRunning = false;
            if (_tempCheckTimer != null)
            {
                _tempCheckTimer.Dispose();
            }

            if (_stopTimer != null)
            {
                _stopTimer.Dispose();
            }

            if (ManagerMode == ManagerMode.Auto)
            {
                TurnOff();
            }

            _progress?.Report(100);
        }
    }
}
