namespace BlazorBreweryInterface.Interfaces
{
    public interface IThermometerController
    {
        DateTime LastReadTime { get; }
        double LastTemperature { get; }
        bool IsThermometerDeviceAvailable();
        Task<double> ReadTemperature();

    }
}
