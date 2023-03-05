using BlazorBrewery.Core.Common;
using BlazorBrewery.Core.Models.Configuration;
using BlazorBrewery.Database.Context;
using BlazorBrewery.Database.Entities;
using BlazorBrewery.Database.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlazorBrewery.Database.Repositories
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly ConfigContext _configContext;

        public ConfigRepository(ConfigContext configContext)
        {
            _configContext = configContext;
        }

        public async Task<List<ConfigItem>> GetConfigItems()
        {
            var output = new List<ConfigItem>();
            var list = await _configContext.Configurationitems.ToListAsync();
            foreach (var item in list)
            {
                output.Add(Parse(item));
            }

            return output;
        }

        private static ConfigItem Parse(ConfigEntity entity)
        {
            return new ConfigItem
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Value = entity.Value
            };
        }

        public int GetPumpPinId()
        {
            return GetPinId(Constants.PumpPinId);
        }

        public int GetHeadingPinId()
        {
            return GetPinId(Constants.HeatingPinId);
        }

        private int GetPinId(string configName)
        {
            var pindIdString = _configContext.Configurationitems.FirstOrDefault(_ => _.Name == configName);
            if (pindIdString == null) { return -1; }

            if (int.TryParse(pindIdString.Value, out var pinId))
            {
                return pinId;
            }

            return -1;
        }
    }
}
