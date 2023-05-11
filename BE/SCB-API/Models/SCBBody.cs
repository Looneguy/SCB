namespace SCB_API.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// <para>
    /// Creates the http body for a query in the structure that SCB wants on their API
    /// </para>
    /// </summary>
    public class SCBBody
    {
        [JsonProperty("query", Required = Required.Always)]
        public Query[] Query { get; set; }

        [JsonProperty("response", Required = Required.Always)]
        public Response Response { get; set; }
    }

    public class Query
    {
        [JsonProperty("code", Required = Required.Always)]
        public string Code { get; set; }

        [JsonProperty("selection", Required = Required.Always)]
        public Selection Selection { get; set; }
    }

    public class Selection
    {
        /// <value>Property <c>Filter</c> | accepted filters are "all","item","top","agg","vs"</value>
        [JsonProperty("filter", Required = Required.Always)]
        public string Filter { get; set; }

        [JsonProperty("values", Required = Required.Always)]
        public string[] Values { get; set; }
    }

    public class Response
    {   /// <value>Property <c>Format</c> | accepted response formats are "px","csv", "json", "xlsx", "json-stat", "json-stat2", "sdmx"</value>
        [JsonProperty("format")]
        public string Format { get; set; }
    }
}
