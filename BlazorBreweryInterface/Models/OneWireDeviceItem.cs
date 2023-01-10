namespace BlazorBreweryInterface.Models
{
    public class OneWireDeviceItem
    {
        public string? BusId { get; set; }

        public string? DeviceId { get; set; }

        public bool IsThermometerDevice { get; set; }
    }
}
