using BlazorBrewery.Database.Interfaces.Repositories;
using BlazorBreweryServer.Services.Interfaces.ViewModels.Settings;
using BlazorBreweryServer.ViewModels.Settings;

namespace BlazorBreweryServer.Services.ViewModels.Settings
{
    public class SettingsViewModelService : ISettingsViewModelService
    {
        private readonly IRecipeRepository _recipeRepository;

        public SettingsViewModelService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<SettingsViewModel> GetSettingsViewModel()
        {
            await _recipeRepository.AllBrewingRecipes();

            return new SettingsViewModel();
        }
    }
}
