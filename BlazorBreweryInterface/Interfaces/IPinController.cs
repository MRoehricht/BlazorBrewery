namespace BlazorBreweryInterface.Interfaces
{
    public interface IPinController : IDisposable
    {
        bool IsOn { get; }
        void Shift(bool isOn, int pinId);
        void Shift(int pinId);
        void SetPinId(int pinId);
    }
}
