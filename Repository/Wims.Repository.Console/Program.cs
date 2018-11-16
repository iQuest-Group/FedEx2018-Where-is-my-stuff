using System;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;
using WIMS.Repository;

namespace Wims.Repository
{
    class Program
    {
        private const string EndpointUrl = "https://fedex2018-wims.documents.azure.com:443/";
        private const string PrimaryKey = "j0EEpoNHkH7wfNACiuxT5eucjCju0vOSyl5aTVuwlpfRY2hRylTbf0OjNtGEMkcpRhnB3jL1JhuwLrNLptfnbQ==";
        private const string DatabaseName = "Inventory";

        static void Main(string[] args)
        {
            Program p = new Program();
            try
            {
                p.GetStartedDemo().Wait();
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                p.WriteToConsoleAndPromptToContinue("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                p.WriteToConsoleAndPromptToContinue("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally
            {
                p.WriteToConsoleAndPromptToContinue("End of demo, press any key to exit.");
            }
        }

        private async Task GetStartedDemo()
        {
            /*create database with collections and demo data*/
            WIMSRepository repository = new WIMSRepository(EndpointUrl, PrimaryKey, DatabaseName);
            //await repository.CreateDatabase();
            await repository.CreateCollection("Products");
            await repository.AddEntities("Products", ProductRepository.GetDemoProducts());

            //await repository.CreateCollection("Inventories");
            //await repository.AddEntities("Inventories", InventoryRepository.GetDemoInventories());

            //read all product IDs
            /*
            foreach (Entity product in repository.GetAllRecordsFromCollection("Products"))
            {
                Console.WriteLine("Product: {0}", product.ID);
            }
            */

            //update inventory
            /*
            InventoryRepository inventoryRepository = new InventoryRepository(EndpointUrl, PrimaryKey, DatabaseName);
            Inventory inventory = inventoryRepository.GetInventory( new Guid("b8775943-5e1e-4097-bc8b-9c937784ed56"));
            Console.WriteLine("Found Inventory {0} - {1} - {2} - {3} - {4} ", inventory.ID, inventory.Description, inventory.StartDate, inventory.EndDate, inventory.Status);

            //...add new tags
            await inventoryRepository.AddNewTagsToInventory(inventory, new string[] { "Product 2 TAG 1" });

            //...close inventory
            await inventoryRepository.CloseInventory(inventory);
            inventory = inventoryRepository.GetInventory(new Guid("b8775943-5e1e-4097-bc8b-9c937784ed56"));
            Console.WriteLine("Found Inventory {0} - {1} - {2} - {3} - {4} ", inventory.ID, inventory.Description, inventory.StartDate, inventory.EndDate, inventory.Status);
            */

        }

        private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }
    }
}
