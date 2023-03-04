using BlazorBreweryInterface.Interfaces;
using BlazorBreweryInterface.Models;

namespace BlazorBreweryInterface.Fake.Controller
{
    public class FakeOneWireDeviceController : IOneWireDeviceController
    {
        public List<OneWireDeviceItem> GetConnectOneWireDevices()
        {
            return new List<OneWireDeviceItem> { new OneWireDeviceItem { BusId = "00000000", DeviceId = "11111111", IsThermometerDevice = true } };
        }
    }
}
