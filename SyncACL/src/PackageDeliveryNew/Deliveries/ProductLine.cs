using System.Collections.Generic;
using PackageDeliveryNew.Common;

namespace PackageDeliveryNew.Deliveries
{
    public class ProductLine : ValueObject<ProductLine>
    {
        public Product Product { get; }
        public int Amount { get; }

        public ProductLine(Product product, int amount)
        {
            Contracts.Require(product != null);
            Contracts.Require(amount >= 0);

            Product = product;
            Amount = amount;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Product;
            yield return Amount;
        }
    }
}
