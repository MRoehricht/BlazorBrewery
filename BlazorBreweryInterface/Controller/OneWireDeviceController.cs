using BlazorBreweryInterface.Interfaces;
using BlazorBreweryInterface.Models;
using Iot.Device.OneWire;

namespace BlazorBreweryInterface.Controller
{
    public class OneWireDeviceController : IOneWireDeviceController
    {
        public List<OneWireDeviceItem> GetConnectOneWireDevices()
        {
            var output = new List<OneWireDeviceItem>();

            foreach (string busId in OneWireBus.EnumerateBusIds())
            {
                OneWireBus bus = new(busId);
                bus.ScanForDeviceChanges();
                foreach (string devId in bus.EnumerateDeviceIds())
                {
                    var item = new OneWireDeviceItem { BusId = bus.BusId, DeviceId = devId, IsThermometerDevice = OneWireThermometerDevice.IsCompatible(busId, devId) };
                    output.Add(item);
                }
            }

            return output;
        }
    }
}
