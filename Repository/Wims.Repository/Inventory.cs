using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace WIMS_Repository
{
    public class Inventory: Entity
    {
        public String Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public String[] ProductTags { get; set; }
        public InventoryStatus Status { get; set; }
    }

    public enum InventoryStatus
    {
        Open = 1,
        Closed
    }
}
