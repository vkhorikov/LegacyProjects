using PackageDelivery.Delivery;

namespace PackageDelivery
{
    public partial class App
    {
        public App()
        {
            DBHelper.Init(@"Server=.\Sql2014;Database=LegacyPrepare;Trusted_Connection=true;");
        }
    }
}
