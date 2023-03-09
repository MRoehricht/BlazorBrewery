namespace BlazorBrewery.Core.Services
{
    public class ConfigurationStoreService : IConfigurationStoreService
    {
        public int HeatPinId { get; init; }
        public int PumpPinId { get; init; }


        public ConfigurationStoreService(string? heatPinId, string? pumpPinId)
        {
            if (int.TryParse(heatPinId, out var pinId))
            {
                HeatPinId = pinId;
            }
            else
            {
                ThrowApplicationException("HeatPinId");
            }

            if (int.TryParse(pumpPinId, out pinId))
            {
                PumpPinId = pinId;
            }
            else
            {
                ThrowApplicationException("PumpPinId");
            }
        }

        private void ThrowApplicationException(string pinName)
        {
            throw new ApplicationException($"{pinName} konnte nicht aus den appsettings gelesen werden.");
        }
    }
}
