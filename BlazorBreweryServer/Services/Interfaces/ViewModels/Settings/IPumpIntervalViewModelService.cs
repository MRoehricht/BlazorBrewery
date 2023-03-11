using BlazorBrewery.Core.Models.Brewing;
using BlazorBreweryServer.ViewModels.Settings;

namespace BlazorBreweryServer.Services.Interfaces.ViewModels.Settings
{
    public interface IPumpIntervalViewModelService
    {
        Task<PumpIntervalViewModel> GetPumpIntervalViewModel();

        Task<Pumpinterval> CreateEmtyPumpInterval();

        Task DeletePumpInterval(Pumpinterval interval);

        Task Save(Pumpinterval interval);
    }
}