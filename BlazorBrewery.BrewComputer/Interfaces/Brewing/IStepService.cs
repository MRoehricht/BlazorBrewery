using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Core.Models.Processes;

namespace BlazorBrewery.BrewComputer.Interfaces.Brewing
{
    public interface IStepService
    {
        void Run(BrewingStep brewingStep, IStepProcessesUpdater updater, CancellationToken cancellationToken);
    }
}