using System;
using System.Threading;
using System.Threading.Tasks;

namespace Acl
{
    public class Program
    {
        private const string LegacyDatabaseConnectionString = @"Server=.\Sql;Database=PackageDelivery;Trusted_Connection=true;";
        private const string BubbleDatabaseConnectionString = @"Server=.\Sql;Database=PackageDeliveryNew;Trusted_Connection=true;";

        private static readonly TimeSpan IntervalBetweenDeliverySyncs = TimeSpan.FromSeconds(1);
        private static readonly TimeSpan IntervalBetweenProductSyncs = TimeSpan.FromHours(1);

        private static Task _deliverySyncThread;
        private static DeliverySyncronizer _deliverySyncronizer;
        private static Task _productSyncThread;
        private static ProductSyncronizer _productSyncronizer;
        private static CancellationTokenSource _cancellationTokenSource;

        public static void Main(string[] args)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _deliverySyncronizer = new DeliverySyncronizer(LegacyDatabaseConnectionString, BubbleDatabaseConnectionString);
            _deliverySyncThread = new Task(
                () => Sync(_deliverySyncronizer.Sync, IntervalBetweenDeliverySyncs),
                TaskCreationOptions.LongRunning);
            _deliverySyncThread.Start();

            _productSyncronizer = new ProductSyncronizer();
            _productSyncThread = new Task(
                () => Sync(_productSyncronizer.Sync, IntervalBetweenProductSyncs),
                TaskCreationOptions.LongRunning);
            _productSyncThread.Start();

            Console.WriteLine("[Press any key to stop]");
            Console.ReadKey();

            _cancellationTokenSource.Cancel();
            _deliverySyncThread.Wait();
            _productSyncThread.Wait();
        }

        private static async void Sync(Action doSync, TimeSpan intervalBetweenSyncs)
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    doSync();
                    await Task.Delay(intervalBetweenSyncs, _cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                }
                catch (Exception ex)
                {
                    Log(ex);
                    throw;
                }
            }
        }

        private static void Log(Exception exception)
        {
            // Log the exception
        }
    }
}
