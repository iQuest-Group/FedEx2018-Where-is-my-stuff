using Newtonsoft.Json;
using System;

namespace iQuest.Fedex2018.Wims.Repository
{
    public class StockItem : Entity
    {
        public string ProductTag { get; set; }
        public DateTime CreateDate { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
