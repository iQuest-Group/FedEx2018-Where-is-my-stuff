using Newtonsoft.Json;
using System.Collections.Generic;

namespace iQuest.Fedex2018.Wims.Repository
{
    public class Product: Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<StockItem> Stock { get; set; }
        

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
