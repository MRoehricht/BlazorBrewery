using BlazorBrewery.Core.Common;
using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Database.Interfaces.Repositories;
using BlazorBreweryInterface.Interfaces;

namespace BlazorBrewery.BrewComputer.Manager
{
    public class PumpManager : IPumpManager, IRelayManagerConsumer
    {
        private readonly IPinController _pumpPinController;
        private readonly IConfigRepository _configRepository;
        private readonly IRelayManager _relayManager;
        private static Timer _timer;
        private bool _pumpIsRunning;
        private readonly int _pumpPinId = -1;
        private Pumpinterval _pumpinterval;

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

        public PumpManager(IPinController heatingPinController, IConfigRepository configRepository, IRelayManager relayManager)
        {

            _pumpPinController = heatingPinController;
            _configRepository = configRepository;
            _relayManager = relayManager;


            _pumpPinId = _configRepository.GetPumpPinId();
            if (_pumpPinId != -1)
            {
                _pumpPinController.SetPinId(_pumpPinId);
            }
            relayManager.Register(this);
        }

        public void Work(Pumpinterval pumpinterval, BrewingStepTyp brewingStepTyp)
        {
            if (_pumpPinId == -1 || brewingStepTyp == BrewingStepTyp.Manually || ManagerMode != ManagerMode.Auto || pumpinterval == null) return;

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
            if (_timer != null)
                _timer.Dispose();
            TurnOff();
        }

        public void StartTimer()
        {
            if (_pumpinterval == null)
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
            _pumpIsRunning = Constants.ON;
            _pumpPinController.Shift(Constants.ON, _pumpPinId);
            _relayManager.SetPinState(_pumpPinId, Constants.ON);
        }

        private void TurnOff()
        {
            _pumpIsRunning = Constants.OFF;
            _pumpPinController.Shift(Constants.OFF, _pumpPinId);
            _relayManager.SetPinState(_pumpPinId, Constants.OFF);

        }

        public void StateChanged(int pinId, bool state)
        {

        }

        public void ModeChanged(int pinId, ManagerMode managerMode)
        {
            if (pinId != _pumpPinId || pinId == -1) return;
            SwitchMode(managerMode);
        }
    }
}
