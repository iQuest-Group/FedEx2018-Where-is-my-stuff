using CosmosDBGettingStarted_WIMS.Properties;
using Microsoft.Azure.Documents;
using System;
using System.Threading.Tasks;
using System.Timers;
using WIMS.Repository;

namespace iQuest.Fedex2018.Winms.TagsFilesProcessor
{
    internal class RecognizedTagsFilesProcessor : IDisposable
    {
        private bool disposed;
        private WIMSRepository repository;
        private readonly string inventoryName;
        private Timer fileCheckerTimer;

        internal RecognizedTagsFilesProcessor(string inventoryNameParam)
        {
            inventoryName = inventoryNameParam;
        }

        internal async Task Start()
        {
            try
            {
                repository = new WIMSRepository(
                    Settings.Default.AzureCosmosDBEndpointUrl,
                    Settings.Default.AzureCosmodBDPrimaryKey,
                    Settings.Default.AzureCosmosDatabaseName);

                await repository.EnsureCollection("Products");

                fileCheckerTimer = new Timer(Settings.Default.FileCheckerTimerIntervalInSeconds * 2000)
                {
                    AutoReset = true,
                    Enabled = true
                };
                fileCheckerTimer.Elapsed += CheckFiles;                
                fileCheckerTimer.Start();
                Console.WriteLine(string.Format("File checker timer started with '{0}' seconds interval", Settings.Default.FileCheckerTimerIntervalInSeconds));
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                WriteToConsoleAndPromptToContinue("Azure DB error occurred: {0}, {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                WriteToConsoleAndPromptToContinue("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
        }

        private void CheckFiles(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Checking files");
        }

        private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing && fileCheckerTimer != null)
            {
                try
                {
                    fileCheckerTimer.Stop();
                    Console.WriteLine("File checker timer stopped");
                }
                catch (Exception exc)
                {
                    Console.WriteLine(string.Format("Exception during stopin the file watcher timer: {0}",
                        exc.Message));
                }
            }

            disposed = true;
        }
    }
}
