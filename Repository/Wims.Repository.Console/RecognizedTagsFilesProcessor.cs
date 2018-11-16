using CosmosDBGettingStarted_WIMS.Properties;
using Microsoft.Azure.Documents;
using System;
using System.Threading.Tasks;
using WIMS.Repository;

namespace iQuest.Fedex2018.Winms.TagsFilesProcessor
{
    internal class RecognizedTagsFilesProcessor
    {
        private WIMSRepository repository;

        internal async Task Start()
        {
            try
            {
                repository = new WIMSRepository(
                    Settings.Default.AzureCosmosDBEndpointUrl,
                    Settings.Default.AzureCosmodBDPrimaryKey,
                    Settings.Default.AzureCosmosDatabaseName);

                await repository.EnsureCollection("Products");
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
            finally
            {
                WriteToConsoleAndPromptToContinue("End of demo, press any key to exit.");
            }
        }

        private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }
    }
}
