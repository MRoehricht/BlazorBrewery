using BlazorBreweryInterface.Models;

namespace BlazorBreweryInterface.Interfaces
{
    public interface IOneWireDeviceController
    {
        public List<OneWireDeviceItem> GetConnectOneWireDevices();
    }
}
