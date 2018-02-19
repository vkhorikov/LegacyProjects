using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace PackageDelivery.Delivery
{
    public static class DBHelper
    {
        private static string _connectionString;

        public static void Init(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static IReadOnlyList<Cstm> GetAllCustomers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT p.*, LTRIM(RTRIM(a.STR)) + ', ' + a.CT_ST ADDR FROM [dbo].[P_TBL] p
                    INNER JOIN [dbo].[ADDR_TBL] a ON p.ADDR1 = a.ID_CLM";
                return connection.Query<Cstm>(query).ToList();
            }
        }

        public static void SaveDelivery(Cstm customer, string addressLine, string cityState, string zipCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT [dbo].[DLVR_TBL] (CSTM, STS)
                    VALUES (@CSTM, 'N')
                    DECLARE @ID int
                    SELECT @ID = SCOPE_IDENTITY()
                    INSERT [dbo].[ADDR_TBL] (STR, CT_ST, ZP, DLVR)
                    VALUES (@STR, @CT_ST, @ZP, @ID)";
                connection.Execute(query, new { CSTM = customer.NMB_CLM, STR = addressLine, CT_ST = cityState, ZP = zipCode });
            }
        }

        public static IReadOnlyList<Dlvr> GetAllDeliveries()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT d1.*, d2.*, LTRIM(RTRIM(a.STR)) + ', ' + LTRIM(RTRIM(a.CT_ST)) + ' ' + LTRIM(RTRIM(a.ZP)) ADDRESS, LTRIM(RTRIM(c.NM_CLM)) CSTM_NAME
                    FROM [dbo].[DLVR_TBL] d1
                    LEFT JOIN [dbo].[DLVR_TBL2] d2 ON d1.[NMB_CLM] = d2.[NMB_CLM]
                    INNER JOIN [dbo].[ADDR_TBL] a ON a.DLVR = d1.NMB_CLM
                    INNER JOIN [dbo].[P_TBL] c ON d1.CSTM = c.NMB_CLM";
                return connection.Query<Dlvr>(query).ToList();
            }
        }

        public static IReadOnlyList<Prdct> GetAllProducts()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM [dbo].[PRD_TBL]";
                return connection.Query<Prdct>(query).ToList();
            }
        }

        public static Prdct GetProduct(int productId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM [dbo].[PRD_TBL] WHERE NMB_CM = @ID";
                return connection.Query<Prdct>(query, new { ID = productId }).SingleOrDefault();
            }
        }

        public static void UpdateDelivery(int id, Prdct product1, int amount1, Prdct product2, int amount2, Prdct product3, int amount3, Prdct product4, int amount4, double costEstimate)
        {
            using (var connection = new SqlConnection(_connectionString))
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

                connection.Execute(query, new
                {
                    NMB_CLM = id,
                    PRD_LN_1 = product1?.NMB_CM,
                    PRD_LN_1_AMN = amount1,
                    PRD_LN_2 = product2?.NMB_CM,
                    PRD_LN_2_AMN = amount2,
                    PRD_LN_3 = product3?.NMB_CM,
                    PRD_LN_3_AMN = amount3,
                    PRD_LN_4 = product4?.NMB_CM,
                    PRD_LN_4_AMN = amount4,
                    ESTM_CLM = costEstimate
                });
            }
        }

        public static void UpdateStatus(int id, string status)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE [dbo].[DLVR_TBL]
                    SET STS = @STS
                    WHERE NMB_CLM = @NMB_CLM";

                connection.Execute(query, new
                {
                    NMB_CLM = id,
                    STS = status
                });
            }
        }
    }
}
