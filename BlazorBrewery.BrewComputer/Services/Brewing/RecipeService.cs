using BlazorBrewery.BrewComputer.Interfaces.Brewing;
using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Core.Models.Processes;

namespace BlazorBrewery.BrewComputer.Services.Brewing
{
    public class RecipeService
    {
        private readonly IStepService _stepService;

        public RecipeService(IStepService stepService) => _stepService = stepService;

        public void Run(BrewingRecipe brewingRecipe, IStepProcessesUpdater updater, CancellationToken cancellationToken)
        {
            foreach (var step in brewingRecipe.BrewingSteps)
            {
                _stepService.Run(step, updater, cancellationToken);
            }
        }
    }
}
