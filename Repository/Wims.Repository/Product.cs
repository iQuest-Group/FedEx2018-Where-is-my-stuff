using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WIMS.Repository
{
    public class Product: Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Stock> Stock { get; set; }
        

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
