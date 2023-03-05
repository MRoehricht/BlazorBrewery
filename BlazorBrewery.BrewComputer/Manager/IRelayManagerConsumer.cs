using BlazorBrewery.Core.Models.Brewing;

namespace BlazorBrewery.BrewComputer.Manager
{
    public interface IRelayManagerConsumer
    {
        void StateChanged(int pinId, bool state);

        void ModeChanged(int pinId, ManagerMode managerMode);
    }
}
