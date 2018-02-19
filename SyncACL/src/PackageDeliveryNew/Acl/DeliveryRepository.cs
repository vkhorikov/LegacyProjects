using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using PackageDeliveryNew.Deliveries;
using PackageDeliveryNew.Utils;

namespace PackageDeliveryNew.Acl
{
    public class DeliveryRepository
    {
        public Delivery GetById(int id)
        {
            DeliveryLegacy legacyDelivery = GetLegacyDelivery(id);
            Delivery delivery = MapLegacyDelivery(legacyDelivery);

            return delivery;
        }

        private Delivery MapLegacyDelivery(DeliveryLegacy legacyDelivery)
        {
            if (legacyDelivery.CT_ST == null || !legacyDelivery.CT_ST.Contains(" "))
                throw new Exception("Invalid city and state");

            string[] cityAndState = legacyDelivery.CT_ST.Split(' ');

            var address = new Address(
                (legacyDelivery.STR ?? "").Trim(),
                cityAndState[0].Trim(),
                cityAndState[1].Trim(),
                (legacyDelivery.ZP ?? "").Trim());

           // return new Delivery(legacyDelivery.NMB_CM, address);
            return null;
        }

        private DeliveryLegacy GetLegacyDelivery(int id)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            {
                string query = @"
                    SELECT d.NMB_CLM, a.*
                    FROM [dbo].[DLVR_TBL] d
                    INNER JOIN [dbo].[ADDR_TBL] a ON a.DLVR = d.NMB_CLM
                    WHERE d.NMB_CLM = @ID";

                return connection
                    .Query<DeliveryLegacy>(query, new { ID = id })
                    .SingleOrDefault();
            }
        }

        private class DeliveryLegacy
        {
            public int NMB_CM { get; set; }
            public string STR { get; set; }
            public string CT_ST { get; set; }
            public string ZP { get; set; }
        }
    }
}
