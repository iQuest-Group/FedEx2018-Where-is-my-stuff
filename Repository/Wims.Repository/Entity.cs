using Newtonsoft.Json;

namespace WIMS.Repository
{
    public class Entity
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
    }
}
