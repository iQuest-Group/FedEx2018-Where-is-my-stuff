using System;
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
            inventory.ProductTags = inventory.ProductTags.Concat(newTags).ToArray();
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
