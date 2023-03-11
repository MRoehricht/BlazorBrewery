using BlazorBrewery.BrewComputer.Manager;
using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Core.Models.Processes;

namespace BlazorBrewery.BrewComputer.Interfaces.Brewing
{
    public interface IStepBrewService
    {
        ITemperatureManager TemperatureManager { get; }
        void Clear();
        Task Run(BrewingStep brewingStep, IStepProcessesUpdater updater, IProgress<int> progress);

        void Stop();

        Action WorkIsDone { get; set; }

        TimeSpan BrewTime { get; }
    }
}