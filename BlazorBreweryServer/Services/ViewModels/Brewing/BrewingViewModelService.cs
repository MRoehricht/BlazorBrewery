using BlazorBrewery.Database.Interfaces.Repositories;
using BlazorBreweryServer.Services.Interfaces.ViewModels.Brewing;
using BlazorBreweryServer.ViewModels.Brewing;

namespace BlazorBreweryServer.Services.ViewModels.Brewing
{
    public class BrewingViewModelService : IBrewingViewModelService
    {
        private readonly IRecipeRepository _recipeRepository;

        public BrewingViewModelService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<BrewingViewModel> GetBrewingViewModel()
        {
            var recipes = await _recipeRepository.AllBrewingRecipes();

            return new BrewingViewModel { Recipes = recipes };
        }

        public async Task SaveViewModel(BrewingViewModel viewModel)
        {
            if (viewModel.SelectedRecipe == null) return;
            await _recipeRepository.Save(viewModel.SelectedRecipe);
        }
    }
}
