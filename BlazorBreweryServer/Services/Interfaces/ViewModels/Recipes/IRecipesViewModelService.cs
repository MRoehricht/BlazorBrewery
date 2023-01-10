using BlazorBreweryServer.ViewModels.Recipes;

namespace BlazorBreweryServer.Services.Interfaces.ViewModels.Recipes
{
    public interface IRecipesViewModelService
    {
        Task<RecipesViewModel> GetRecipes();
    }
}