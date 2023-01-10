using BlazorBrewery.Database.Interfaces.Repositories;
using BlazorBreweryServer.Services.Interfaces.ViewModels.Recipes;
using BlazorBreweryServer.ViewModels.Recipes;

namespace BlazorBreweryServer.Services.ViewModels.Recipes
{
    public class RecipesViewModelService : IRecipesViewModelService
    {
        private readonly IRecipeRepository _recipeRepository;

        public RecipesViewModelService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipesViewModel> GetRecipes()
        {
            var recipes = await _recipeRepository.AllBrewingRecipes();

            return new RecipesViewModel { Recipes = recipes };
        }
    }
}
