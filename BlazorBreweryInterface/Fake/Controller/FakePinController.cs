using BlazorBreweryInterface.Interfaces;
using System.Device.Gpio;
using System.Diagnostics;

namespace BlazorBreweryInterface.Fake.Controller
{
    public class FakePinController : IPinController
    {
        private int _pinId;
        private bool _isOn;
        public bool IsOn { get => _isOn; }


        public void Shift(bool isOn, int pinId)
        {
            _isOn = isOn;
            _pinId = pinId;
            string pinValue = isOn ? PinValue.High.ToString() : PinValue.Low.ToString();

            var status = _isOn ? "AN" : "AUS";

            if (_pinId == 14)
            {
                Trace.WriteLine($"{DateTime.Now.ToLongTimeString()} Pumpe   {status}");
            }
            if (_pinId == 15)
            {
                Trace.WriteLine($"{DateTime.Now.ToLongTimeString()} Heizung {status}");
            }


        }

        public void Shift(int pinId) => Shift(!_isOn, pinId);
        public void Dispose() => Shift(false, _pinId);

        public void SetPinId(int pinId)
        {
            _pinId = pinId;
        }
    }
}

