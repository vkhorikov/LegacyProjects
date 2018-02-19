using PackageDelivery.Delivery;
using PackageDeliveryNew.Utils;

namespace PackageDelivery
{
    public partial class App
    {
        public App()
        {
            string legacyDatabaseConnectionString = @"Server=.\Sql;Database=PackageDelivery;Trusted_Connection=true;";
            string bubbleDatabaseConnectionString = @"Server=.\Sql;Database=PackageDeliveryNew;Trusted_Connection=true;";

            DBHelper.Init(legacyDatabaseConnectionString);
            Settings.Init(bubbleDatabaseConnectionString);
        }
    }
}
