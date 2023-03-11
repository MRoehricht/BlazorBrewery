using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Database.Interfaces.Repositories;
using BlazorBreweryServer.Services.Interfaces.ViewModels.Settings;
using BlazorBreweryServer.ViewModels.Settings;

namespace BlazorBreweryServer.Services.ViewModels.Settings
{
    public class PumpIntervalViewModelService : IPumpIntervalViewModelService
    {
        private readonly IRecipeRepository _recipeRepository;

        public PumpIntervalViewModelService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<PumpIntervalViewModel> GetPumpIntervalViewModel()
        {
            var model = new PumpIntervalViewModel
            {
                Pumpintervals = await _recipeRepository.GetAllPumpintervals()
            };

            return model;
        }

        public async Task<Pumpinterval> CreateEmtyPumpInterval()
        {
            var pumpInterval = await _recipeRepository.CreateEmptyPumpInterval();
            return pumpInterval;
        }

        public async Task DeletePumpInterval(Pumpinterval interval)
        {
            await _recipeRepository.Delete(interval);
        }

        public async Task Save(Pumpinterval interval)
        {
            await _recipeRepository.Save(interval);
        }
    }
}
