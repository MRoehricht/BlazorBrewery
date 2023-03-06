using BlazorBrewery.Core.Common;
using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Database.Interfaces.Repositories;
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
        private readonly IConfigRepository _configRepository;
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

        public Action WorkDone { get; set; }
        private Timer _stopTimer;
        private Timer _tempCheckTimer;

        public TemperatureManager(IThermometerController thermometerController, IConfigRepository configRepository, IRelayManager relayManager, ILogger<TemperatureManager> logger)
        {
            _thermometerController = thermometerController;
            _configRepository = configRepository;
            _relayManager = relayManager;
            _logger = logger;

            if (!_thermometerController.IsThermometerDeviceAvailable())
            {
                throw new ApplicationException("ThermometerDevice is not available!");
            }

            _heatPinId = _configRepository.GetHeadingPinId();
            if (_heatPinId != -1)
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (env != null && env == "Development")
                {
                    _heatingPinController = new FakePinController(_heatPinId);
                }
                else
                {
                    _heatingPinController = new PinController(_heatPinId);
                }
            }
            else
            {
                throw new ApplicationException("HeatPinId is not set in Database!");
            }

            relayManager.Register(this);
        }

        public void Work(double targetTemperature, int durationMinutes, BrewingStepTyp brewingStepTyp)
        {
            if (_heatPinId == -1 || brewingStepTyp == BrewingStepTyp.Manually) return;
            _targetTemperature = targetTemperature;
            _brewingStepTyp = brewingStepTyp;
            _durationMinutes = durationMinutes;
            _isRunning = true;
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
            if (_tempCheckTimer != null)
            {
                _tempCheckTimer.Dispose();
            }

            TurnOff();
            WorkDone?.Invoke();
        }

        public async Task CheckTemperature()
        {
            if (ManagerMode != ManagerMode.Auto) return;
            var currentTemperature = await GetCurrentTemperature();
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
                if (_tempCheckTimer != null)
                {
                    _tempCheckTimer.Dispose();
                }
                TurnOn();

            }
            else if (mode == ManagerMode.Off)
            {
                if (_tempCheckTimer != null)
                {
                    _tempCheckTimer.Dispose();
                }
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
        }
    }
}
