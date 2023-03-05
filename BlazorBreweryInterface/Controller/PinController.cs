using BlazorBreweryInterface.Interfaces;
using System.Device.Gpio;

namespace BlazorBreweryInterface.Controller
{
    public class PinController : IPinController
    {
        private int _pinId;
        private bool _isOn;

        public bool IsOn { get => _isOn; }

        public void Shift(bool isOn, int pinId)
        {
            _isOn = isOn;
            _pinId = pinId;
            using var controller = new GpioController();
            controller.OpenPin(_pinId, PinMode.Output);
            controller.Write(_pinId, isOn ? PinValue.High : PinValue.Low);
        }

        public void Shift(int pinId) => Shift(!_isOn, pinId);

        public void Dispose() => Shift(false, _pinId);

        public void SetPinId(int pinId)
        {
            _pinId = pinId;
        }
    }
}
