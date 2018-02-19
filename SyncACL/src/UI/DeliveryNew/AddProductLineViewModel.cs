using PackageDelivery.Common;
using PackageDeliveryNew.Deliveries;

namespace PackageDelivery.DeliveryNew
{
    public class AddProductLineViewModel : ViewModel
    {
        public Product Product { get; private set; }
        public string ProductName => Product == null ? string.Empty : Product.Name;
        public int Amount { get; set; }

        public Command ChangeProductCommand { get; }
        public Command OkCommand { get; }
        public Command CancelCommand { get; }

        public override string Caption => "Add product line";
        public override double Height => 170;

        public AddProductLineViewModel()
        {
            OkCommand = new Command(CanSave, () => DialogResult = true);
            CancelCommand = new Command(() => DialogResult = false);
            ChangeProductCommand = new Command(ChangeProduct);
        }

        private void ChangeProduct()
        {
            var viewModel = new ChangeProductViewModel();

            if (_dialogService.ShowDialog(viewModel) == true)
            {
                Product = viewModel.SelectedProduct;
                Notify(nameof(ProductName));
            }
        }

        private bool CanSave() => Product != null && Amount > 0;
    }
}
