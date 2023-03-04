using BlazorBreweryInterface.Fake.Controller;
using BlazorBreweryInterface.Interfaces;

namespace BlazorBreweryInterface.Test.Controller
{
    public class PinControllerTest
    {
        private IPinController _pinController;

        [SetUp]
        public void Setup()
        {
            _pinController = new FakePinController();
        }

        [Test]
        public void IsOnTest()
        {
            Assert.That(_pinController.IsOn, Is.False);
            _pinController.Shift(true, 1);
            Assert.That(_pinController.IsOn, Is.True);
            _pinController.Shift(1);
            Assert.That(_pinController.IsOn, Is.False);
            _pinController.Shift(1);
            Assert.That(_pinController.IsOn, Is.True);
            _pinController.Shift(false, 1);
            Assert.That(_pinController.IsOn, Is.False);
        }

        [Test]
        public void DisposeTest()
        {
            _pinController.Shift(true, 1);
            _pinController.Dispose();
            Assert.That(_pinController.IsOn, Is.False);
        }
    }
}
