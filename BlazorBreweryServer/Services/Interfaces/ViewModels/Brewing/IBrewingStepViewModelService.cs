using BlazorBrewery.Core.Models.Brewing;
using BlazorBreweryServer.ViewModels.Brewing;

namespace BlazorBreweryServer.Services.Interfaces.ViewModels.Brewing
{
    public interface IBrewingStepViewModelService
    {
        Task<BrewingStepViewModel> GetBrewingStepViewModel(Guid stepId);

        List<BrewingStepViewModel> GetBrewingStepsViewModel(BrewingRecipe brewingRecipe);
    }
}
