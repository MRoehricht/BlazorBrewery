using BlazorBrewery.Core.Models.Brewing;

namespace BlazorBrewery.BrewComputer.Manager
{
    public interface IRelayManager
    {
        ManagerMode GetPinMode(int pinId);
        bool GetPinState(int pinId);
        void Register(IRelayManagerConsumer relayManagerConsumer);
        void SetPinMode(int pinId, ManagerMode managerMode);
        void SetPinState(int pinId, bool state);
    }
}