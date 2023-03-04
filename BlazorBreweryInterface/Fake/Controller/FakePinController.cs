using BlazorBreweryInterface.Interfaces;
using System.Device.Gpio;

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
            Console.WriteLine($"Pin {_pinId} {pinValue}");
        }

        public void Shift(int pinId) => Shift(!_isOn, pinId);
        public void Dispose() => Shift(false, _pinId);
    }
}

