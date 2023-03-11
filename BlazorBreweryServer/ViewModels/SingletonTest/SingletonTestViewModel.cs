namespace BlazorBreweryServer.ViewModels.SingletonTest
{
    public class SingletonTestViewModel
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; RefreshMe?.Invoke(); }
        }



        public Action RefreshMe { get; set; }
    }
}
