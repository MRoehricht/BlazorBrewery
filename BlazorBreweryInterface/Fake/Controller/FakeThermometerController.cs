using BlazorBreweryInterface.Interfaces;

namespace BlazorBreweryInterface.Fake.Controller
{
    public class FakeThermometerController : IThermometerController
    {
        private const string _fileName = "CurrentTemp.txt";


        public bool IsThermometerDeviceAvailable()
        {
            return true;
        }

        public async Task<double> ReadTemperature()
        {
            if (!File.Exists(_fileName))
            {
                await File.WriteAllTextAsync(_fileName, "15");
            }

            var text = await File.ReadAllTextAsync(_fileName);
            return double.Parse(text);
        }
    }
}
