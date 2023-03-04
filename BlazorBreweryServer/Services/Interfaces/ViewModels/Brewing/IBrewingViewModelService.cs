using BlazorBreweryServer.ViewModels.Brewing;

namespace BlazorBreweryServer.Services.Interfaces.ViewModels.Brewing
{
    public interface IBrewingViewModelService
    {
        Task<BrewingViewModel> GetBrewingViewModel();

        Task SaveViewModel(BrewingViewModel viewModel);
    }
}
