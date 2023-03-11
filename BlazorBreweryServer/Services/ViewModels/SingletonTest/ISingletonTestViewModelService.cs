using BlazorBreweryServer.ViewModels.SingletonTest;

namespace BlazorBreweryServer.Services.ViewModels.SingletonTest
{
    public interface ISingletonTestViewModelService
    {
        SingletonTestViewModel GetViewModelInstanz();
    }
}