using BlazorBrewery.Core.Common;
using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Core.Services;
using BlazorBreweryInterface.Controller;
using BlazorBreweryInterface.Fake.Controller;
using BlazorBreweryInterface.Interfaces;
using Microsoft.Extensions.Logging;

namespace BlazorBrewery.BrewComputer.Manager
{
    public class PumpManager : IPumpManager, IRelayManagerConsumer
    {
        private readonly IPinController _pumpPinController;
        private readonly IRelayManager _relayManager;
        private readonly ILogger<PumpManager> _logger;
        private static Timer _timer;
        private bool _pumpIsRunning;
        private readonly int _pumpPinId = -1;
        private Pumpinterval? _pumpinterval;
        private bool _isRunning = false;

        private ManagerMode _managerMode = ManagerMode.Auto;
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

        public int PinId => _pumpPinId;

        public PumpManager(IRelayManager relayManager, ILogger<PumpManager> logger, IConfigurationStoreService configurationStoreService)
        {
            _relayManager = relayManager;
            _logger = logger;
            _pumpPinId = configurationStoreService.PumpPinId;
            if (_pumpPinId != -1)
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (env != null && env == "Development")
                {
                    _pumpPinController = new FakePinController(_pumpPinId);
                }
                else
                {
                    _pumpPinController = new PinController(_pumpPinId);
                }
            }
            else
            {
                throw new ApplicationException("PumpPinId ist auf -1 gestellt. Bitte Konfiguration prüfen.");
            }
            relayManager.Register(this);
        }

        public void Work(Pumpinterval pumpinterval, BrewingStepTyp brewingStepTyp)
        {
            if (_pumpPinId == -1 || brewingStepTyp == BrewingStepTyp.Manually || ManagerMode != ManagerMode.Auto || pumpinterval == null) return;
            _isRunning = true;
            _pumpinterval = pumpinterval;
            if (pumpinterval.PausetimeSeconds == 0 && pumpinterval.RuntimeSeconds > 0)
            {
                TurnOn();
                return;
            }

            if (pumpinterval.RuntimeSeconds == 0)
            {
                TurnOff();
                return;
            }

            StartTimer();
        }

        public void StopWork()
        {
            _isRunning = false;
            if (_timer != null)
                _timer.Dispose();

            if (ManagerMode == ManagerMode.Auto)
            {
                TurnOff();
            }
        }

        public void StartTimer()
        {
            if (_pumpinterval == null || _isRunning == false)
            {
                TurnOff();
                return;
            }

            if (_pumpinterval.PausetimeSeconds == 0 && _pumpinterval.RuntimeSeconds > 0)
            {
                TurnOn();
                return;
            }

            if (_pumpinterval.RuntimeSeconds == 0)
            {
                TurnOff();
                return;
            }


            _timer = new Timer(
               callback: new TimerCallback(TimerCallbackTask),
               state: _pumpinterval,
               dueTime: _pumpinterval.RuntimeMilliSeconds,
               period: 0);
            TurnOn();
        }

        private void TimerCallbackTask(object? state)
        {
            if (_pumpIsRunning)
            {
                _timer.Change(_pumpinterval.PausetimeMilliSeconds, 0);
                TurnOff();
            }
            else
            {
                _timer.Change(_pumpinterval.RuntimeMilliSeconds, 0);
                TurnOn();
            }
        }

        private void SwitchMode(ManagerMode mode)
        {
            if (_pumpPinId == -1) return;

            if (mode == ManagerMode.On)
            {
                if (_timer != null)
                    _timer.Dispose();
                TurnOn();
            }
            else if (mode == ManagerMode.Off)
            {
                if (_timer != null)
                    _timer.Dispose();
                TurnOff();
            }
            else if (mode == ManagerMode.Auto)
            {
                if (_timer != null)
                    _timer.Dispose();
                StartTimer();
            }
        }

        private void TurnOn()
        {
            if (_pumpPinController.IsOn) return;
            _pumpIsRunning = Constants.ON;
            _pumpPinController.Shift(Constants.ON);
            _relayManager.SetPinState(_pumpPinId, Constants.ON);
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - Pumpe AN");
        }

        private void TurnOff()
        {
            if (!_pumpPinController.IsOn) return;
            _pumpIsRunning = Constants.OFF;
            _pumpPinController.Shift(Constants.OFF);
            _relayManager.SetPinState(_pumpPinId, Constants.OFF);
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - Pumpe AUS");

        }

        public void StateChanged(int pinId, bool state)
        {

        }

        public void ModeChanged(int pinId, ManagerMode managerMode)
        {
            if (pinId != _pumpPinId || pinId == -1) return;
            ManagerMode = managerMode;
        }
    }
}
