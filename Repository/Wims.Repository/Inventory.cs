using System;

namespace WIMS.Repository
{
    public class Inventory: Entity
    {
        public String Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public String[] ProductTags { get; set; }
        public InventoryStatus Status { get; set; }

        public void EnsureProductTags()
        {
            if (ProductTags == null)
            {
                ProductTags = new string[0];
            }
        }
    }

    public enum InventoryStatus
    {
        Open = 1,
        Closed
    }
}
