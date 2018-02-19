using PackageDelivery.Common;
using PackageDeliveryNew.Common;
using PackageDeliveryNew.Deliveries;

namespace PackageDelivery.Delivery
{
    public class EditPackageViewModel : ViewModel
    {
        private readonly Dlvr _delivery;

        private Prdct _product1;
        public string Product1Name => _product1 == null ? string.Empty : _product1.NM_CLM;
        public int Amount1 { get; set; }

        private Prdct _product2;
        public string Product2Name => _product2 == null ? string.Empty : _product2.NM_CLM;
        public int Amount2 { get; set; }

        private Prdct _product3;
        public string Product3Name => _product3 == null ? string.Empty : _product3.NM_CLM;
        public int Amount3 { get; set; }

        private Prdct _product4;
        public string Product4Name => _product4 == null ? string.Empty : _product4.NM_CLM;
        public int Amount4 { get; set; }

        public double CostEstimate { get; set; }

        public Command ChangeProduct1Command { get; }
        public Command ChangeProduct2Command { get; }
        public Command ChangeProduct3Command { get; }
        public Command ChangeProduct4Command { get; }
        public Command RecalculateCostCommand { get; }

        public Command OkCommand { get; }
        public Command CancelCommand { get; }

        public override string Caption => "Edit Package";
        public override double Height => 410;

        private readonly EstimateCalculator _estimateCalculator;

        public EditPackageViewModel(Dlvr delivery)
        {
            _delivery = delivery;
            _product1 = _delivery.PRD_LN_1 == null ? null : DBHelper.GetProduct(_delivery.PRD_LN_1.Value);
            _product2 = _delivery.PRD_LN_2 == null ? null : DBHelper.GetProduct(_delivery.PRD_LN_2.Value);
            _product3 = _delivery.PRD_LN_3 == null ? null : DBHelper.GetProduct(_delivery.PRD_LN_3.Value);
            _product4 = _delivery.PRD_LN_4 == null ? null : DBHelper.GetProduct(_delivery.PRD_LN_4.Value);
            Amount1 = _delivery.PRD_LN_1_AMN == null ? 0 : int.Parse(_delivery.PRD_LN_1_AMN);
            Amount2 = _delivery.PRD_LN_2_AMN == null ? 0 : int.Parse(_delivery.PRD_LN_2_AMN);
            Amount3 = _delivery.PRD_LN_3_AMN == null ? 0 : int.Parse(_delivery.PRD_LN_3_AMN);
            Amount4 = _delivery.PRD_LN_4_AMN == null ? 0 : int.Parse(_delivery.PRD_LN_4_AMN);
            CostEstimate = _delivery.ESTM_CLM;

            OkCommand = new Command(Save);
            CancelCommand = new Command(() => DialogResult = false);
            ChangeProduct1Command = new Command(() => ChangeProduct(ref _product1, nameof(Product1Name)));
            ChangeProduct2Command = new Command(() => ChangeProduct(ref _product2, nameof(Product2Name)));
            ChangeProduct3Command = new Command(() => ChangeProduct(ref _product3, nameof(Product3Name)));
            ChangeProduct4Command = new Command(() => ChangeProduct(ref _product4, nameof(Product4Name)));
            RecalculateCostCommand = new Command(RecalculateCost);

            _estimateCalculator = new EstimateCalculator();
        }

        private void RecalculateCost()
        {
            Result<decimal> estimateOrError = _estimateCalculator.Calculate(
                _delivery.NMB_CLM,
                _product1?.NMB_CM, Amount1,
                _product2?.NMB_CM, Amount2,
                _product3?.NMB_CM, Amount3,
                _product4?.NMB_CM, Amount4);

            if (estimateOrError.IsFailure)
            {
                CustomMessageBox.ShowError(estimateOrError.Error);
                return;
            }

            CostEstimate = (double)estimateOrError.Value;

            Notify(nameof(CostEstimate));
        }

        private void ChangeProduct(ref Prdct product, string propertyToNotify)
        {
            var viewModel = new ChangeProductViewModel();

            if (_dialogService.ShowDialog(viewModel) == true)
            {
                product = viewModel.SelectedProduct;
                Notify(propertyToNotify);
            }
        }

        private void Save()
        {
            DBHelper.UpdateDelivery(_delivery.NMB_CLM, _product1, Amount1, _product2, Amount2, _product3, Amount3, _product4, Amount4, CostEstimate);
            DialogResult = true;
        }
    }
}
