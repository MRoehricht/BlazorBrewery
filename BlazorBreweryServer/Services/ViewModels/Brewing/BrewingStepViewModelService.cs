using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Database.Interfaces.Repositories;
using BlazorBreweryServer.Services.Interfaces.ViewModels.Brewing;
using BlazorBreweryServer.ViewModels.Brewing;

namespace BlazorBreweryServer.Services.ViewModels.Brewing
{
    public class BrewingStepViewModelService : IBrewingStepViewModelService
    {

        private readonly IRecipeRepository _recipeRepository;

        public BrewingStepViewModelService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }


        public async Task<BrewingStepViewModel> GetBrewingStepViewModel(Guid stepId)
        {
            var step = await _recipeRepository.GetBrewingStep(stepId);
            return new BrewingStepViewModel { BrewingStep = step };
        }

        public List<BrewingStepViewModel> GetBrewingStepsViewModel(BrewingRecipe brewingRecipe)
        {
            List<BrewingStepViewModel> brewingStepViewModels = new List<BrewingStepViewModel>();

            foreach (var step in brewingRecipe.BrewingSteps)
            {
                brewingStepViewModels.Add(new BrewingStepViewModel { BrewingStep = step });
            }

            return brewingStepViewModels;
        }
    }
}
