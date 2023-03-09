using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Core.Models.Processes;

namespace BlazorBrewery.BrewComputer.Interfaces.Brewing
{
    public interface IStepBrewService
    {
        void Clear();
        void Run(BrewingStep brewingStep, IStepProcessesUpdater updater, IProgress<int> progress);

        void Stop();

        Action WorkIsDone { get; set; }

        TimeSpan BrewTime { get; }
    }
}