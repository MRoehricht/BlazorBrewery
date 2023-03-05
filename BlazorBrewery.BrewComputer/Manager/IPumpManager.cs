using BlazorBrewery.Core.Models.Brewing;

namespace BlazorBrewery.BrewComputer.Manager
{
    public interface IPumpManager
    {
        int PinId { get; }

        ManagerMode ManagerMode { get; set; }

        void Work(Pumpinterval pumpinterval, BrewingStepTyp brewingStepTyp);

        void StopWork();
    }
}
