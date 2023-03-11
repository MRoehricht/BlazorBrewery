using BlazorBreweryInterface.Interfaces;
using System.Device.Gpio;

namespace BlazorBreweryInterface.Controller
{
    public class PinController : IPinController
    {
        private readonly int _pinId;
        private bool _isOn;

        public bool IsOn { get => _isOn; }

        public PinController(int pinId)
        {
            _pinId = pinId;
            Shift(false);
        }

        public void Shift(bool isOn)
        {
            if (isOn == _isOn) return;
            _isOn = isOn;
            using var controller = new GpioController();
            controller.OpenPin(_pinId, PinMode.Output);
            controller.Write(_pinId, isOn ? PinValue.Low : PinValue.High);
        }

        public void Shift() => Shift(!_isOn);

        public void Dispose() => Shift(false);
    }
}
