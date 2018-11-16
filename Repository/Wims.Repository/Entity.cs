using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WIMS.Repository
{
    public class Entity
    {
        [JsonProperty(PropertyName = "id")]
        public Guid ID { get; set; }
    }
}
