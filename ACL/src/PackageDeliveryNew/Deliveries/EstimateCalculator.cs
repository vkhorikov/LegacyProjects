using System;
using System.Collections.Generic;
using System.Linq;
using PackageDeliveryNew.Acl;
using PackageDeliveryNew.Common;

namespace PackageDeliveryNew.Deliveries
{
    public class EstimateCalculator
    {
        private readonly DeliveryRepository _deliveryRepository;
        private readonly ProductRepository _productRepository;
        private readonly AddressResolver _addressResolver;

        public EstimateCalculator()
        {
            _deliveryRepository = new DeliveryRepository();
            _productRepository = new ProductRepository();
            _addressResolver = new AddressResolver();
        }

        public Result<decimal> Calculate(int deliveryId, int? productId1, int amount1,
            int? productId2, int amount2, int? productId3, int amount3, int? productId4, int amount4)
        {
            if (productId1 == null && productId2 == null && productId3 == null && productId4 == null)
                return Result.Fail<decimal>("Must provide at least 1 product");

            Delivery delivery = _deliveryRepository.GetById(deliveryId);
            if (delivery == null)
                throw new Exception("Delivery is not found for Id: " + deliveryId);

            double? distance = _addressResolver.GetDistanceTo(delivery.Address);
            if (distance == null)
                return Result.Fail<decimal>("Address is not found");

            List<ProductLine> productLines = new List<(int? productId, int amount)>
                {
                    (productId1, amount1),
                    (productId2, amount2),
                    (productId3, amount3),
                    (productId4, amount4)
                }
                .Where(x => x.productId != null)
                .Select(x => new ProductLine(_productRepository.GetById(x.productId.Value), x.amount))
                .ToList();

            if (productLines.Any(x => x.Product == null))
                throw new Exception("One of the products is not found");

            return Result.Ok(delivery.GetEstimate(distance.Value, productLines));
        }
    }

    public class AddressResolver
    {
        public double? GetDistanceTo(Address address)
        {
            /* Call to an external API */
            return 15;
        }
    }
}
