using BlazorBrewery.Core.Models.Brewing;

namespace BlazorBrewery.BrewComputer.Manager
{
    public class RelayManager : IRelayManager
    {
        private readonly Dictionary<int, ManagerMode> _pinModes;
        private readonly Dictionary<int, bool> _pinState;
        private readonly List<IRelayManagerConsumer> _relayManagerConsumers;

        public RelayManager()
        {
            _pinModes = new Dictionary<int, ManagerMode>();
            _pinState = new Dictionary<int, bool>();
            _relayManagerConsumers = new List<IRelayManagerConsumer>();
        }

        public void Register(IRelayManagerConsumer relayManagerConsumer)
        {
            _relayManagerConsumers.Add(relayManagerConsumer);
        }

        public void SetPinMode(int pinId, ManagerMode managerMode)
        {
            if (_pinModes.ContainsKey(pinId))
            {
                _pinModes[pinId] = managerMode;
            }
            else
            {
                _pinModes.Add(pinId, managerMode);
            }

            _relayManagerConsumers.ForEach(relayManagerConsumer => relayManagerConsumer.ModeChanged(pinId, managerMode));
        }

        public ManagerMode GetPinMode(int pinId)
        {
            if (_pinModes.TryGetValue(pinId, out var mode))
            {
                return mode;
            }

            _pinModes.Add(pinId, ManagerMode.Auto);
            return ManagerMode.Auto;
        }

        public void SetPinState(int pinId, bool state)
        {
            if (_pinState.ContainsKey(pinId))
            {
                _pinState[pinId] = state;
            }
            else
            {
                _pinState.Add(pinId, state);
            }
            _relayManagerConsumers.ForEach(relayManagerConsumer => relayManagerConsumer.StateChanged(pinId, state));
        }

        public bool GetPinState(int pinId)
        {
            if (_pinState.TryGetValue(pinId, out var state))
            {
                return state;
            }

            _pinState.Add(pinId, false);
            return false;
        }
    }
}
