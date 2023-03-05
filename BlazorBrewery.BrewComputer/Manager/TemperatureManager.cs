using BlazorBrewery.Core.Common;
using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Database.Interfaces.Repositories;
using BlazorBreweryInterface.Interfaces;

namespace BlazorBrewery.BrewComputer.Manager
{
    public class TemperatureManager : ITemperatureManager, IRelayManagerConsumer
    {
        private readonly IThermometerController _thermometerController;
        private readonly IPinController _heatingPinController;
        private readonly IConfigRepository _configRepository;
        private readonly IRelayManager _relayManager;
        private int HeatPinId = -1;

        private ManagerMode _managerMode = ManagerMode.Auto;
        private double _targetTemperature;
        private BrewingStepTyp _brewingStepTyp;

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

        public int PinId => HeatPinId;

        public Action WorkDone { get; set; }
        private Timer _stopTimer;
        private Timer _tempCheckTimer;

        public TemperatureManager(IThermometerController thermometerController, IPinController heatingPinController, IConfigRepository configRepository, IRelayManager relayManager)
        {
            _thermometerController = thermometerController;
            _heatingPinController = heatingPinController;
            _configRepository = configRepository;
            _relayManager = relayManager;
            if (!_thermometerController.IsThermometerDeviceAvailable())
            {
                throw new ApplicationException("ThermometerDevice is not available!");
            }

            HeatPinId = _configRepository.GetHeadingPinId();
            if (HeatPinId != -1)
            {
                _heatingPinController.SetPinId(HeatPinId);
            }
            relayManager.Register(this);
        }

        public void Work(double targetTemperature, int durationSeconds, BrewingStepTyp brewingStepTyp)
        {
            if (HeatPinId == -1 || brewingStepTyp == BrewingStepTyp.Manually || ManagerMode != ManagerMode.Auto) return;
            _targetTemperature = targetTemperature;
            _brewingStepTyp = brewingStepTyp;

            if (durationSeconds > 0)
            {
                _stopTimer = new Timer(
               callback: new TimerCallback(StopWorkingCallback),
               state: null,
               dueTime: (durationSeconds * 1000),
               period: 0);
            }

            _tempCheckTimer = new Timer(
               callback: new TimerCallback(CheckTemperatureCallback),
               state: null,
               dueTime: 2000,
               period: 0);

        }

        private void CheckTemperatureCallback(object? state)
        {

            _ = CheckTemperature();
        }

        private void StopWorkingCallback(object? state)
        {
            if (_tempCheckTimer != null)
            {
                _tempCheckTimer.Dispose();
            }

            TurnOff();
            if (WorkDone != null)
            {
                WorkDone();
            }
        }

        public async Task CheckTemperature()
        {
            var currentTemperature = await GetCurrentTemperature();
            if (!IsInHysteresis(_targetTemperature, currentTemperature, _heatingPinController.IsOn))
            {
                if (_brewingStepTyp == BrewingStepTyp.HoldTemperature || _brewingStepTyp == BrewingStepTyp.Heat)
                {
                    if (currentTemperature < _targetTemperature)
                    {
                        TurnOn();
                    }
                    else
                    {
                        TurnOff();
                    }
                }

                if (_brewingStepTyp == BrewingStepTyp.CoolDown)
                {
                    TurnOff();
                }
            }
            else
            {
                if (_brewingStepTyp == BrewingStepTyp.Heat)
                {
                    StopWorkingCallback(null);
                }
            }
        }

        public static bool IsInHysteresis(double targetTemperature, double currentTemperature, bool isHeating)
        {
            if (isHeating)
            {
                return currentTemperature < targetTemperature + 1;
            }

            return currentTemperature < targetTemperature - 1;
        }

        public async Task<double> GetCurrentTemperature()
        {
            var currentTemperature = await _thermometerController.ReadTemperature();
            return currentTemperature;
        }

        private void SwitchMode(ManagerMode mode)
        {
            if (HeatPinId == -1) return;

            if (mode == ManagerMode.On)
            {
                TurnOn();
            }
            else if (mode == ManagerMode.Off || mode == ManagerMode.Auto)
            {
                TurnOff();
            }
        }

        private void TurnOn()
        {
            _heatingPinController.Shift(Constants.ON, HeatPinId);
            _relayManager.SetPinState(HeatPinId, Constants.ON);
        }

        private void TurnOff()
        {
            _heatingPinController.Shift(Constants.OFF, HeatPinId);
            _relayManager.SetPinState(HeatPinId, Constants.OFF);

        }

        public void StateChanged(int pinId, bool state)
        {

        }

        public void ModeChanged(int pinId, ManagerMode managerMode)
        {
            if (pinId != HeatPinId || pinId == -1) return;
            SwitchMode(managerMode);
        }

        public void StopWork()
        {

        }
    }
}
