using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Core.Models.Processes;

namespace BlazorBrewery.BrewComputer.Services.Brewing
{
    public class RecipeBrewService
    {
        private readonly Interfaces.Brewing.IStepBrewService _stepService;

        public RecipeBrewService(Interfaces.Brewing.IStepBrewService stepService) => _stepService = stepService;

        public void Run(BrewingRecipe brewingRecipe, IStepProcessesUpdater updater, CancellationToken cancellationToken)
        {
            foreach (var step in brewingRecipe.BrewingSteps)
            {



                _stepService.Run(step, updater, null);
            }
        }
    }
}
