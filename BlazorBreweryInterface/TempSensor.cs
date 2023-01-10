using Iot.Device.OneWire;

namespace BlazorBreweryInterface
{
    public class TempSensor
    {

        public async Task<string> Quick()
        {
            // Quick and simple way to find a thermometer and print the temperature
            foreach (var dev in OneWireThermometerDevice.EnumerateDevices())
            {
                var temperatur = (await dev.ReadTemperatureAsync()).DegreesCelsius.ToString("F2");
                Console.WriteLine($"Temperature reported by '{dev.DeviceId}': " +
                                     temperatur + "\u00B0C");

                return temperatur;
            }

            return "Unbekannt";
        }


        public async Task<string> ReadSensorAdress()
        {
            Console.WriteLine($"ReadSensorAdress");
            foreach (string busId in OneWireBus.EnumerateBusIds())
            {
                OneWireBus bus = new(busId);
                Console.WriteLine($"Found bus '{bus.BusId}', scanning for devices ...");
                await bus.ScanForDeviceChangesAsync();
                foreach (string devId in bus.EnumerateDeviceIds())
                {
                    OneWireDevice dev = new(busId, devId);
                    Console.WriteLine($"Found family '{dev.Family}' device '{dev.DeviceId}' on '{bus.BusId}'");
                    if (OneWireThermometerDevice.IsCompatible(busId, devId))
                    {
                        OneWireThermometerDevice devTemp = new(busId, devId);
                        var temoerature = (await devTemp.ReadTemperatureAsync()).DegreesCelsius.ToString("F2");
                        Console.WriteLine("Temperature reported by device: " +
                                            temoerature +
                                            "\u00B0C");

                        return temoerature;
                    }
                }
            }

            return "Ubekannt";
        }
    }
}
