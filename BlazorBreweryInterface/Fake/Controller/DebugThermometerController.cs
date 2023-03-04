using BlazorBreweryInterface.Interfaces;

namespace BlazorBreweryInterface.Fake.Controller
{
    public class FakeThermometerController : IThermometerController
    {
        public bool IsThermometerDeviceAvailable()
        {
            throw new NotImplementedException();
        }

        public Task<double> ReadTemperature()
        {
            throw new NotImplementedException();
        }
    }
}
