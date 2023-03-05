using BlazorBrewery.Core.Models.Configuration;

namespace BlazorBrewery.Database.Interfaces.Repositories
{
    public interface IConfigRepository
    {
        Task<List<ConfigItem>> GetConfigItems();

        int GetPumpPinId();

        int GetHeadingPinId();
    }
}
