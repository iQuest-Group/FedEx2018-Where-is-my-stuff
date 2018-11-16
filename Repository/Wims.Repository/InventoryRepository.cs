using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WIMS.Repository
{
    public class InventoryRepository : WimsRepository<Inventory>
    {
        public InventoryRepository(string EndpointUrl, string PrimaryKey, string DatabaseName) :
            base(EndpointUrl, PrimaryKey, DatabaseName)
        {
        }

        protected override string CollectionName => "Inventories";

        public async Task AddNewTagsToInventory(Inventory inventory, string[] newTags)
        {
            inventory.EnsureProductTags();
            HashSet<string> tagsSet = new HashSet<string>();

            foreach (string tag in inventory.ProductTags)
            {
                tagsSet.Add(tag);
            }
            foreach (string tag in newTags)
            {
                tagsSet.Add(tag);
            }
            inventory.ProductTags = tagsSet.ToArray();
            await docClient.ReplaceDocumentAsync(CreateDocumentUri(inventory), inventory);
        }

        public async Task CloseInventory(Inventory inventory)
        {
            inventory.Status = InventoryStatus.Closed;
            inventory.EndDate = DateTime.UtcNow;
            await docClient.ReplaceDocumentAsync(CreateDocumentUri(inventory), inventory);
        }
    }
}
