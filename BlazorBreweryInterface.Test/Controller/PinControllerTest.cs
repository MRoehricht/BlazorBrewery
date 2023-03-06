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
            _pinController = new FakePinController(1);
        }

        [Test]
        public void IsOnTest()
        {
            Assert.That(_pinController.IsOn, Is.False);
            _pinController.Shift(true);
            Assert.That(_pinController.IsOn, Is.True);
            _pinController.Shift();
            Assert.That(_pinController.IsOn, Is.False);
            _pinController.Shift();
            Assert.That(_pinController.IsOn, Is.True);
            _pinController.Shift(false);
            Assert.That(_pinController.IsOn, Is.False);
        }

        [Test]
        public void DisposeTest()
        {
            _pinController.Shift(true);
            _pinController.Dispose();
            Assert.That(_pinController.IsOn, Is.False);
        }
    }
}
