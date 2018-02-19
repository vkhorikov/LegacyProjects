using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using PackageDeliveryNew.Deliveries;
using PackageDeliveryNew.Utils;

namespace PackageDeliveryNew.Acl
{
    public class ProductRepository
    {
        private const double PoundsInKilogram = 2.20462;

        public Product GetById(int id)
        {
            ProductLegacy legacyProduct = GetLegacyProduct(id);
            Product product = MapLegacyProduct(legacyProduct);

            return product;
        }

        private Product MapLegacyProduct(ProductLegacy legacyProduct)
        {
            if (legacyProduct.WT == null && legacyProduct.WT_KG == null)
                throw new Exception("Invalid weight in product: " + legacyProduct.NMB_CM);

            double weightInPounds = legacyProduct.WT ?? legacyProduct.WT_KG.Value * PoundsInKilogram;
            return null; // new Product(legacyProduct.NMB_CM, weightInPounds);
        }

        private ProductLegacy GetLegacyProduct(int id)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            {
                string query = @"
                    SELECT NMB_CM, WT, WT_KG
                    FROM [dbo].[PRD_TBL]
                    WHERE NMB_CM = @ID";

                return connection
                    .Query<ProductLegacy>(query, new { ID = id })
                    .SingleOrDefault();
            }
        }

        private class ProductLegacy
        {
            public int NMB_CM { get; set; }
            public double? WT { get; set; }
            public double? WT_KG { get; set; }
        }
    }
}
