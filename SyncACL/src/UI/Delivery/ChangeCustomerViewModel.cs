using System.Collections.Generic;
using PackageDelivery.Common;

namespace PackageDelivery.Delivery
{
    public class ChangeCustomerViewModel : ViewModel
    {
        public IReadOnlyList<Cstm> Customers { get; }
        public Cstm SelectedCustomer { get; set; }
        public Command<Cstm> OkCommand { get; }
        public Command CancelCommand { get; }

        public override string Caption => "Change customer";

        public ChangeCustomerViewModel()
        {
            Customers = DBHelper.GetAllCustomers();
            OkCommand = new Command<Cstm>(d => d != null, _ => DialogResult = true);
            CancelCommand = new Command(() => DialogResult = false);
        }
    }
}
