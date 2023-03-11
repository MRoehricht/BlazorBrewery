using BlazorBreweryServer.ViewModels.SingletonTest;

namespace BlazorBreweryServer.Services.ViewModels.SingletonTest
{
    public class SingletonTestViewModelService : ISingletonTestViewModelService
    {
        private SingletonTestViewModel? _viewModel = null;

        public SingletonTestViewModelService()
        {

        }



        public SingletonTestViewModel GetViewModelInstanz()
        {
            if (_viewModel == null)
            {
                _viewModel = new SingletonTestViewModel { Name = "Matze" };
            }

            return _viewModel;
        }


    }
}
