using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Acl
{
    public class DeliverySyncronizer
    {
        private readonly string _legacyConnectionString;
        private readonly string _bubbleConnectionString;

        public DeliverySyncronizer(string legacyConnectionString, string bubbleConnectionString)
        {
            _legacyConnectionString = legacyConnectionString;
            _bubbleConnectionString = bubbleConnectionString;
        }

        public void Sync()
        {
            SyncFromLegacyToBubble();
            SyncFromBubbleToLegacy();
        }

        private void SyncFromBubbleToLegacy()
        {
            if (!IsSyncFromBubbleNeeded())
                return;

            List<DeliveryBubble> bubbleDeliveries = ReadUpdatedBubbleDeliveries();
            List<DeliveryLegacy> legacyDeliveries = MapBubbleDeliveries(bubbleDeliveries);
            SaveDeliveryDeliveries(legacyDeliveries);
        }

        private void SaveDeliveryDeliveries(List<DeliveryLegacy> legacyDeliveries)
        {
            using (var connection = new SqlConnection(_legacyConnectionString))
            {
                string query = @"
                    UPDATE [dbo].[DLVR_TBL]
                    SET PRD_LN_1 = @PRD_LN_1, PRD_LN_1_AMN = @PRD_LN_1_AMN, PRD_LN_2 = @PRD_LN_2, PRD_LN_2_AMN = @PRD_LN_2_AMN, ESTM_CLM = @ESTM_CLM, STS = 'R'
                    WHERE NMB_CLM = @NMB_CLM

                    IF EXISTS (SELECT TOP 1 1 FROM [dbo].[DLVR_TBL2] WHERE NMB_CLM = @NMB_CLM)
                    BEGIN
                        UPDATE [dbo].[DLVR_TBL2]
                        SET PRD_LN_3 = @PRD_LN_3, PRD_LN_3_AMN = @PRD_LN_3_AMN, PRD_LN_4 = @PRD_LN_4, PRD_LN_4_AMN = @PRD_LN_4_AMN
                        WHERE NMB_CLM = @NMB_CLM
                    END
                    ELSE
                    BEGIN
                        INSERT [dbo].[DLVR_TBL2] (NMB_CLM, PRD_LN_3, PRD_LN_3_AMN, PRD_LN_4, PRD_LN_4_AMN)
                        VALUES (@NMB_CLM, @PRD_LN_3, @PRD_LN_3_AMN, @PRD_LN_4, @PRD_LN_4_AMN)
                    END";

                connection.Execute(query, legacyDeliveries);
            }
        }

        private List<DeliveryLegacy> MapBubbleDeliveries(List<DeliveryBubble> bubbleDeliveries)
        {
            var result = new List<DeliveryLegacy>();

            foreach (DeliveryBubble bubbleDelivery in bubbleDeliveries)
            {
                var legacyDelivery = new DeliveryLegacy
                {
                    NMB_CLM = bubbleDelivery.DeliveryID,
                    ESTM_CLM = (double?)bubbleDelivery.CostEstimate
                };
                if (bubbleDelivery.Lines.Count > 0)
                {
                    legacyDelivery.PRD_LN_1 = bubbleDelivery.Lines[0].ProductID;
                    legacyDelivery.PRD_LN_1_AMN = bubbleDelivery.Lines[0].Amount.ToString();
                }
                if (bubbleDelivery.Lines.Count > 1)
                {
                    legacyDelivery.PRD_LN_2 = bubbleDelivery.Lines[1].ProductID;
                    legacyDelivery.PRD_LN_2_AMN = bubbleDelivery.Lines[1].Amount.ToString();
                }
                if (bubbleDelivery.Lines.Count > 2)
                {
                    legacyDelivery.PRD_LN_3 = bubbleDelivery.Lines[2].ProductID;
                    legacyDelivery.PRD_LN_3_AMN = bubbleDelivery.Lines[2].Amount.ToString();
                }
                if (bubbleDelivery.Lines.Count > 3)
                {
                    legacyDelivery.PRD_LN_4 = bubbleDelivery.Lines[3].ProductID;
                    legacyDelivery.PRD_LN_4_AMN = bubbleDelivery.Lines[3].Amount.ToString();
                }
                result.Add(legacyDelivery);
            }

            return result;
        }

        private List<DeliveryBubble> ReadUpdatedBubbleDeliveries()
        {
            using (var connection = new SqlConnection(_bubbleConnectionString))
            {
                string query = @"
                    SELECT d.DeliveryID, d.CostEstimate
                    FROM [dbo].[Delivery] d WITH (UPDLOCK)
                    WHERE d.IsSyncNeeded = 1

                    SELECT l.*
                    FROM [dbo].[Delivery] d
                    INNER JOIN [dbo].[ProductLine] l ON d.DeliveryID = l.DeliveryID
                    WHERE d.IsSyncNeeded = 1

                    UPDATE [dbo].[Delivery]
                    SET IsSyncNeeded = 0
                    WHERE IsSyncNeeded = 1

                    UPDATE [dbo].[Synchronization]
                    SET IsSyncRequired = 0";

                SqlMapper.GridReader reader = connection.QueryMultiple(query);
                List<DeliveryBubble> deliveries = reader.Read<DeliveryBubble>().ToList();
                List<ProductLineBubble> lines = reader.Read<ProductLineBubble>().ToList();

                foreach (DeliveryBubble delivery in deliveries)
                {
                    delivery.Lines = lines.Where(x => x.DeliveryID == delivery.DeliveryID).ToList();
                }

                return deliveries;
            }
        }

        private bool IsSyncFromBubbleNeeded()
        {
            using (var connection = new SqlConnection(_bubbleConnectionString))
            {
                string query = @"SELECT IsSyncRequired FROM [dbo].[Synchronization]";
                return connection.Query<bool>(query).Single();
            }
        }

        private void SyncFromLegacyToBubble()
        {
            if (!IsSyncFromLegacyNeeded())
                return;

            List<DeliveryLegacy> updatedDeliveries = ReadUpdatedLegacyDeliveries();
            List<DeliveryBubble> bubbleDeliveries = MapLegacyDeliveries(updatedDeliveries);
            SaveBubbleDeliveries(bubbleDeliveries);
        }

        private void SaveBubbleDeliveries(List<DeliveryBubble> bubbleDeliveries)
        {
            using (var connection = new SqlConnection(_bubbleConnectionString))
            {
                string query = @"
                    UPDATE [dbo].[Delivery]
                    SET DestinationStreet = @DestinationStreet,
                        DestinationCity = @DestinationCity,
                        DestinationState = @DestinationState,
                        DestinationZipCode = @DestinationZipCode
                    WHERE DeliveryID = @DeliveryID

                    IF (@@ROWCOUNT = 0)
                    BEGIN
                        INSERT [dbo].[Delivery] (DeliveryID, DestinationStreet, DestinationCity, DestinationState, DestinationZipCode)
                        VALUES (@DeliveryID, @DestinationStreet, @DestinationCity, @DestinationState, @DestinationZipCode)
                    END";

                connection.Execute(query, bubbleDeliveries);
            }
        }

        private List<DeliveryBubble> MapLegacyDeliveries(List<DeliveryLegacy> updatedDeliveries)
        {
            return updatedDeliveries.Select(x => MapLegacyDelivery(x)).ToList();
        }

        private DeliveryBubble MapLegacyDelivery(DeliveryLegacy legacyDelivery)
        {
            if (legacyDelivery.CT_ST == null || !legacyDelivery.CT_ST.Contains(" "))
                throw new Exception("Invalid city and state");

            string[] cityAndState = legacyDelivery.CT_ST.Split(' ');

            var delivery = new DeliveryBubble
            {
                DeliveryID = legacyDelivery.NMB_CLM,
                DestinationStreet = (legacyDelivery.STR ?? "").Trim(),
                DestinationCity = cityAndState[0].Trim(),
                DestinationState = cityAndState[1].Trim(),
                DestinationZipCode = (legacyDelivery.ZP ?? "").Trim()
            };

            return delivery;
        }

        private List<DeliveryLegacy> ReadUpdatedLegacyDeliveries()
        {
            using (var connection = new SqlConnection(_legacyConnectionString))
            {
                string query = @"
                    SELECT d.NMB_CLM, a.*
                    FROM [dbo].[DLVR_TBL] d WITH (UPDLOCK)
                    INNER JOIN [dbo].[ADDR_TBL] a ON a.DLVR = d.NMB_CLM
                    WHERE d.IsSyncNeeded = 1

                    UPDATE [dbo].[DLVR_TBL]
                    SET IsSyncNeeded = 0
                    WHERE IsSyncNeeded = 1

                    UPDATE [dbo].[Synchronization]
                    SET IsSyncRequired = 0";

                return connection
                    .Query<DeliveryLegacy>(query)
                    .ToList();
            }
        }

        private bool IsSyncFromLegacyNeeded()
        {
            using (var connection = new SqlConnection(_legacyConnectionString))
            {
                string query = @"SELECT IsSyncRequired FROM [dbo].[Synchronization]";
                return connection.Query<bool>(query).Single();
            }
        }

        private class DeliveryLegacy
        {
            public int NMB_CLM { get; set; }
            public string STR { get; set; }
            public string CT_ST { get; set; }
            public string ZP { get; set; }
            public double? ESTM_CLM { get; set; }
            public int? PRD_LN_1 { get; set; }
            public string PRD_LN_1_AMN { get; set; }
            public int? PRD_LN_2 { get; set; }
            public string PRD_LN_2_AMN { get; set; }
            public int? PRD_LN_3 { get; set; }
            public string PRD_LN_3_AMN { get; set; }
            public int? PRD_LN_4 { get; set; }
            public string PRD_LN_4_AMN { get; set; }
        }

        private class DeliveryBubble
        {
            public int DeliveryID { get; set; }
            public decimal? CostEstimate { get; set; }
            public string DestinationStreet { get; set; }
            public string DestinationCity { get; set; }
            public string DestinationState { get; set; }
            public string DestinationZipCode { get; set; }

            public List<ProductLineBubble> Lines { get; set; }
        }

        private class ProductLineBubble
        {
            public int ProductID { get; set; }
            public int Amount { get; set; }
            public int DeliveryID { get; set; }
        }
    }
}
