using BlazorBreweryInterface.Interfaces;

namespace BlazorBreweryInterface.Fake.Controller
{
    public class FakeThermometerController : IThermometerController
    {
        private const string _fileName = "CurrentTemp.txt";

        public DateTime LastReadTime { get; private set; }

        public double LastTemperature { get; private set; }

        public bool IsThermometerDeviceAvailable()
        {
            return true;
        }

        public async Task<double> ReadTemperature()
        {
            if (DateTime.Now < LastReadTime.AddSeconds(2))
            {
                return LastTemperature;
            }

            if (!File.Exists(_fileName))
            {
                await File.WriteAllTextAsync(_fileName, "15");
            }
            LastReadTime = DateTime.Now;
            var text = await File.ReadAllTextAsync(_fileName);
            LastTemperature = double.Parse(text);
            return LastTemperature;
        }
    }
}
