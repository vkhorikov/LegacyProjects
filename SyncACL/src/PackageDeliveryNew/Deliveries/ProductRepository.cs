using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using PackageDeliveryNew.Utils;

namespace PackageDeliveryNew.Deliveries
{
    public class ProductRepository
    {
        public Product GetById(int id)
        {
            ProductData productData = GetRawData(id);
            Product product = MapData(productData);

            return product;
        }

        private Product MapData(ProductData productData)
        {
            return new Product(productData.ProductID, productData.Name, productData.WeightInPounds);
        }

        private ProductData GetRawData(int id)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            {
                string query = @"
                    SELECT *
                    FROM [dbo].[Product]
                    WHERE ProductID = @ID";

                return connection
                    .Query<ProductData>(query, new { ID = id })
                    .SingleOrDefault();
            }
        }

        private class ProductData
        {
            public int ProductID { get; set; }
            public string Name { get; set; }
            public double WeightInPounds { get; set; }
        }

        public IReadOnlyList<Product> GetAll()
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            {
                string query = @"SELECT * FROM [dbo].[Product]";

                return connection
                    .Query<ProductData>(query)
                    .Select(x => MapData(x))
                    .ToList();
            }
        }
    }
}
