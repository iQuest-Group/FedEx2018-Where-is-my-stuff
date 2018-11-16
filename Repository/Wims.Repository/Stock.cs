using Newtonsoft.Json;
using System;

namespace WIMS_Repository
{
    public class Stock: Entity
    {
        public string ProductTag { get; set; }
        public DateTime CreateDate { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
