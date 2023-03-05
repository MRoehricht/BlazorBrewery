using BlazorBrewery.Core.Models.Brewing;

namespace BlazorBrewery.BrewComputer.Manager
{
    public interface ITemperatureManager
    {
        int PinId { get; }

        ManagerMode ManagerMode { get; set; }

        void Work(double targetTemperature, int durationSeconds, BrewingStepTyp brewingStepTyp);

        Task<double> GetCurrentTemperature();

        void StopWork();

        Action WorkDone { get; set; }
    }
}