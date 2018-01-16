using PackageDelivery.Delivery;

namespace PackageDelivery.Common
{
    public class MainViewModel
    {
        public ViewModel ViewModel { get; }

        public MainViewModel()
        {
            ViewModel = new DeliveryListViewModel();
        }
    }
}
