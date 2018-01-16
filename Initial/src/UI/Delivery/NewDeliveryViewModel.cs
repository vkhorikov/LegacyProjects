using PackageDelivery.Common;

namespace PackageDelivery.Delivery
{
    public class NewDeliveryViewModel : ViewModel
    {
        private Cstm _customer;

        public string CustomerName => _customer == null ? string.Empty : _customer.NM_CLM;
        public string AddressLine { get; set; }
        public string CityState { get; set; }
        public string ZipCode { get; set; }

        public Command ChangeCustomerCommand { get; }
        public Command OkCommand { get; }
        public Command CancelCommand { get; }

        public override string Caption => "New delivery";
        public override double Height => 300;

        public NewDeliveryViewModel()
        {
            OkCommand = new Command(CanSave, Save);
            CancelCommand = new Command(() => DialogResult = false);
            ChangeCustomerCommand = new Command(ChangeCustomer);
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(CustomerName)
                && !string.IsNullOrWhiteSpace(AddressLine)
                && !string.IsNullOrWhiteSpace(CityState) && CityState.Trim().Contains(" ")
                && !string.IsNullOrWhiteSpace(ZipCode);
        }

        private void ChangeCustomer()
        {
            var viewModel = new ChangeCustomerViewModel();

            if (_dialogService.ShowDialog(viewModel) == true)
            {
                _customer = viewModel.SelectedCustomer;
                Notify(() => CustomerName);
            }
        }

        private void Save()
        {
            DBHelper.SaveDelivery(_customer, AddressLine, CityState, ZipCode);
            DialogResult = true;
        }
    }
}
