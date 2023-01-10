namespace BlazorBreweryInterface.Interfaces
{
    public interface IThermometerController
    {
        bool IsThermometerDeviceAvailable();
        Task<double> ReadTemperature();

    }
}
