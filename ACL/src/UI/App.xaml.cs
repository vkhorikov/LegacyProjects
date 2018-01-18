using PackageDelivery.Delivery;

namespace PackageDelivery
{
    public partial class App
    {
        public App()
        {
            DBHelper.Init(@"Server=.\Sql;Database=PackageDelivery;Trusted_Connection=true;");
        }
    }
}
