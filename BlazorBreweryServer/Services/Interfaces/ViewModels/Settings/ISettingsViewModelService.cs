using BlazorBreweryServer.ViewModels.Settings;

namespace BlazorBreweryServer.Services.Interfaces.ViewModels.Settings
{
    public interface ISettingsViewModelService
    {
        Task<SettingsViewModel> GetSettingsViewModel();
    }
}