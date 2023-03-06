using BlazorBreweryInterface.Interfaces;
using System.Device.Gpio;
using System.Diagnostics;

namespace BlazorBreweryInterface.Fake.Controller
{
    public class FakePinController : IPinController
    {
        private readonly int _pinId;
        private bool _isOn;
        public bool IsOn { get => _isOn; }

        public FakePinController(int pinId)
        {
            _pinId = pinId;
        }

        public void Shift(bool isOn)
        {
            if (isOn == _isOn) return;
            _isOn = isOn;
            string pinValue = isOn ? PinValue.High.ToString() : PinValue.Low.ToString();

            var status = _isOn ? "AN" : "AUS";

            if (_pinId == 14)
            {
                Trace.WriteLine($"{DateTime.Now.ToLongTimeString()} Pumpe   {status} - {pinValue}");
            }
            if (_pinId == 15)
            {
                Trace.WriteLine($"{DateTime.Now.ToLongTimeString()} Heizung {status} - {pinValue}");
            }
        }

        public void Shift() => Shift(!_isOn);
        public void Dispose() => Shift(false);
    }
}

