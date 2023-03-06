namespace BlazorBreweryInterface.Interfaces
{
    public interface IPinController : IDisposable
    {
        bool IsOn { get; }
        void Shift(bool isOn);
        void Shift();
    }
}
