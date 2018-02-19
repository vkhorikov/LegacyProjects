using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using PackageDeliveryNew.Utils;

namespace PackageDeliveryNew.Deliveries
{
    public class DeliveryRepository
    {
        public Delivery GetById(int id)
        {
            (DeliveryData deliveryData, List<ProductLineData> linesData) = GetRawData(id);
            Delivery delivery = MapData(deliveryData, linesData);

            return delivery;
        }

        private Delivery MapData(DeliveryData deliveryData, List<ProductLineData> linesData)
        {
            List<ProductLine> lines = linesData.Select(x => new ProductLine(
                    new Product(x.ProductID, x.ProductName, x.ProductWeightInPounds), 
                    x.Amount))
                .ToList();

            var delivery = new Delivery(
                deliveryData.DeliveryID,
                new Address(
                    deliveryData.DestinationStreet,
                    deliveryData.DestinationCity,
                    deliveryData.DestinationState,
                    deliveryData.DestinationZipCode),
                deliveryData.CostEstimate,
                lines);

            return delivery;
        }

        private (DeliveryData deliveryData, List<ProductLineData> linesData) GetRawData(int id)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            {
                string query = @"
                    SELECT *
                    FROM [dbo].[Delivery] d
                    WHERE d.DeliveryID = @ID

                    SELECT l.*, p.WeightInPounds ProductWeightInPounds, p.Name ProductName
                    FROM [dbo].[ProductLine] l
                    INNER JOIN [dbo].[Product] p ON l.ProductID = p.ProductID
                    WHERE l.DeliveryID = @ID";

                SqlMapper.GridReader reader = connection.QueryMultiple(query, new { ID = id });
                DeliveryData deliveryData = reader.Read<DeliveryData>().SingleOrDefault();
                List<ProductLineData> lineData = reader.Read<ProductLineData>().ToList();

                return (deliveryData, lineData);
            }
        }

        public void Save(Delivery delivery)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            {
                string query = @"
                    UPDATE [dbo].[Delivery]
                    SET CostEstimate = @CostEstimate
                    WHERE DeliveryID = @ID

                    DELETE FROM [dbo].[ProductLine]
                    WHERE DeliveryID = @ID";

                connection.Execute(query, new { ID = delivery.Id, delivery.CostEstimate });

                string query2 = @"
                    INSERT INTO [dbo].[ProductLine] (ProductID, Amount, DeliveryID)
                    VALUES (@ProductID, @Amount, @DeliveryID)";
                
                connection.Execute(query2, delivery.Lines.Select(x => 
                    new
                    {
                        ProductID = x.Product.Id,
                        x.Amount,
                        DeliveryID = delivery.Id
                    }));
            }
        }

        private class DeliveryData
        {
            public int DeliveryID { get; set; }
            public decimal? CostEstimate { get; set; }
            public string DestinationStreet { get; set; }
            public string DestinationCity { get; set; }
            public string DestinationState { get; set; }
            public string DestinationZipCode { get; set; }
        }

        private class ProductLineData
        {
            public int ProductID { get; set; }
            public int Amount { get; set; }
            public double ProductWeightInPounds { get; set; }
            public string ProductName { get; set; }
        }
    }
}
