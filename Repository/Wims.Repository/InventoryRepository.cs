using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIMS_Repository
{
    public class InventoryRepository: WIMSRepository
    {
        private const String CollectionName = "Inventories";
        private const String DemoFileName = "Inventory.json";


        public InventoryRepository(string EndpointUrl, string PrimaryKey, string DatabaseName):base(EndpointUrl, PrimaryKey, DatabaseName){ }

        public Inventory GetInventory(Guid id)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Inventory> inventoryQuery = this.docClient.CreateDocumentQuery<Inventory>(
                    UriFactory.CreateDocumentCollectionUri(this.DatabaseName, CollectionName), queryOptions)
                    .Where(i => i.ID == id);

            return inventoryQuery.Take(1).AsEnumerable().First();
            //todo: need to handle the scenario in which no inventory is found
        }
        
       
        public async Task AddNewTagsToInventory(Inventory inventory, string[] newTags)
        {
            inventory.ProductTags = inventory.ProductTags.Concat(newTags).ToArray();
            await this.docClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(this.DatabaseName, CollectionName, inventory.ID.ToString()), inventory);
        }

        public async Task CloseInventory(Inventory inventory)
        {
            inventory.Status = InventoryStatus.Closed;
            inventory.EndDate = DateTime.UtcNow;
            await this.docClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(this.DatabaseName, CollectionName, inventory.ID.ToString()), inventory);
        }


        public static Inventory[] GetDemoInventories()
        {
            List<Inventory> inventories;
            string path = WIMSRepository.DemoFolderPath + DemoFileName;

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                inventories = JsonConvert.DeserializeObject<List<Inventory>>(json);
            }
            return inventories.ToArray();
        }
    }
}
