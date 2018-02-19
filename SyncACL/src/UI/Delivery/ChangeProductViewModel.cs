using System.Collections.Generic;
using PackageDelivery.Common;

namespace PackageDelivery.Delivery
{
    public class ChangeProductViewModel : ViewModel
    {
        public IReadOnlyList<Prdct> Products { get; }
        public Prdct SelectedProduct { get; set; }
        public Command<Prdct> OkCommand { get; }
        public Command CancelCommand { get; }

        public override string Caption => "Change product";

        public ChangeProductViewModel()
        {
            Products = DBHelper.GetAllProducts();
            OkCommand = new Command<Prdct>(x => x != null, _ => DialogResult = true);
            CancelCommand = new Command(() => DialogResult = false);
        }
    }
}