using BlazorBrewery.BrewComputer.Interfaces.Brewing;
using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Core.Models.Processes;

namespace BlazorBrewery.BrewComputer.Services.Brewing
{
    public class StepService : IStepService
    {
        private BrewingStep _brewingStep;

        public void Run(BrewingStep brewingStep, IStepProcessesUpdater updater, CancellationToken cancellationToken)
        {
            _brewingStep = brewingStep;
        }
    }
}
