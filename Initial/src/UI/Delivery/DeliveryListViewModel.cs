using System.Collections.Generic;
using PackageDelivery.Common;

namespace PackageDelivery.Delivery
{
    public class DeliveryListViewModel : ViewModel
    {
        public Command RefreshCommand { get; }
        public Command NewDeliveryCommand { get; }
        public Command<Dlvr> EditPackageCommand { get; }
        public Command<Dlvr> MarkAsInProgressCommand { get; }
        public IReadOnlyList<Dlvr> Deliveries { get; private set; }

        public DeliveryListViewModel()
        {
            RefreshCommand = new Command(Refresh);
            NewDeliveryCommand = new Command(NewDelivery);
            EditPackageCommand = new Command<Dlvr>(x => x != null, EditPackage);
            MarkAsInProgressCommand = new Command<Dlvr>(x => x != null && x.STS == "R", MarkAsInProgress);

            Refresh();
        }

        private void MarkAsInProgress(Dlvr delivery)
        {
            DBHelper.UpdateStatus(delivery.NMB_CLM, "P");

            Refresh();
        }

        private void EditPackage(Dlvr delivery)
        {
            var viewModel = new EditPackageViewModel(delivery);
            _dialogService.ShowDialog(viewModel);

            Refresh();
        }

        private void NewDelivery()
        {
            var viewModel = new NewDeliveryViewModel();
            _dialogService.ShowDialog(viewModel);

            Refresh();
        }

        private void Refresh()
        {
            Deliveries = DBHelper.GetAllDeliveries();

            Notify(nameof(Deliveries));
        }
    }
}
