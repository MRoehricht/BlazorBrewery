using BlazorBrewery.Core.Models.Brewing;

namespace BlazorBrewery.BrewComputer.Manager
{
    public interface ITemperatureManager
    {
        int PinId { get; }

        ManagerMode ManagerMode { get; set; }

        void Work(double targetTemperature, int durationMinutes, BrewingStepTyp brewingStepTyp, IProgress<int> progress);

        Task<double> GetCurrentTemperature();

        void StopWork();

        Action WorkDone { get; set; }

        //Action TempHasChanged { get; set; }
    }
}