using Newtonsoft.Json;

namespace SCB_API.Models
{
    public class BornStatisticDto
    {
        [JsonProperty("data")]
        public Data[] Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("key")]
        public string[] Key { get; set; }

        [JsonProperty("values")]
        public string[] Values { get; set; }

        public DateTime FetchedAt { get; set; } = DateTime.UtcNow;
    }
}
