namespace PackageDeliveryNew.Utils
{
    public static class Settings
    {
        public static string ConnectionString { get; private set; }

        public static void Init(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
