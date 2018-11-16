using Newtonsoft.Json;

namespace iQuest.Fedex2018.Wims.Repository
{
    public class Entity
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
    }
}
