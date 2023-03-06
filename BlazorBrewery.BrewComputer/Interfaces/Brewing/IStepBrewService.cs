using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Core.Models.Processes;

namespace BlazorBrewery.BrewComputer.Interfaces.Brewing
{
    public interface IStepBrewService
    {
        void Run(BrewingStep brewingStep, IStepProcessesUpdater updater);

        void Stop();

        Action WorkIsDone { get; set; }
    }
}