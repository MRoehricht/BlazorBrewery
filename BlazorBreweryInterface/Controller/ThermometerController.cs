﻿using BlazorBreweryInterface.Interfaces;
using Iot.Device.OneWire;

namespace BlazorBreweryInterface.Controller
{
    public class ThermometerController : IThermometerController
    {
        public DateTime LastReadTime { get; private set; }

        public double LastTemperature { get; private set; }

        public bool IsThermometerDeviceAvailable()
        {
            return OneWireThermometerDevice.EnumerateDevices().Any();
        }

        public async Task<double> ReadTemperature()
        {
            if (DateTime.Now < LastReadTime.AddSeconds(2))
            {
                return LastTemperature;
            }

            LastReadTime = DateTime.Now;


            foreach (var dev in OneWireThermometerDevice.EnumerateDevices())
            {
                var temperatur = (await dev.ReadTemperatureAsync()).DegreesCelsius;
                LastTemperature = temperatur;
                break;
            }

            return LastTemperature;
        }

        //public async Task A()
        //{


        //    foreach (string busId in OneWireBus.EnumerateBusIds())
        //    {
        //        OneWireBus bus = new(busId);
        //        Console.WriteLine($"Found bus '{bus.BusId}', scanning for devices ...");
        //        await bus.ScanForDeviceChangesAsync();
        //        foreach (string devId in bus.EnumerateDeviceIds())
        //        {
        //            OneWireDevice dev = new(busId, devId);
        //            Console.WriteLine($"Found family '{dev.Family}' device '{dev.DeviceId}' on '{bus.BusId}'");
        //            if (OneWireThermometerDevice.IsCompatible(busId, devId))
        //            {
        //                OneWireThermometerDevice devTemp = new(busId, devId);
        //                var temoerature = (await devTemp.ReadTemperatureAsync()).DegreesCelsius.ToString("F2");
        //                Console.WriteLine("Temperature reported by device: " +
        //                                    temoerature +
        //                                    "\u00B0C");

        //                return temoerature;
        //            }
        //        }
        //    }
        //}
    }
}
