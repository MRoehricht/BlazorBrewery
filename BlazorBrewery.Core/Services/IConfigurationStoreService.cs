namespace BlazorBrewery.Core.Services
{
    public interface IConfigurationStoreService
    {
        int HeatPinId { get; }
        int PumpPinId { get; }
    }
}
