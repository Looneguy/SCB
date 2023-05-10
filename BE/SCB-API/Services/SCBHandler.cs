using Newtonsoft.Json;
using SCB_API.Models;

namespace SCB_API.Services
{
    public class SCBHandler
    {
        // Only BE will be used in this project and its subcategories.
        // To see examples of subcategories of each subject area
        // do a GET call to https://api.scb.se/OV0104/v1/doris/sv/ssd/{SubjectArea.AnyOfTheCodes}
        public static readonly string AA = "AA"; // General statistics
        public static readonly string AM = "AM"; // Labour market
        public static readonly string BE = "BE"; // Population
        public static readonly string BO = "BO"; // Housing, construction and building
        public static readonly string EN = "EN"; // Energy
        public static readonly string FM = "FM"; // Financial markets
        public static readonly string HA = "HA"; // Trade in goods and services
        public static readonly string HE = "HE"; // Household finances
        public static readonly string JO = "JO"; // Agriculture, forestry and fishery
        public static readonly string LE = "LE"; // Living conditions
        public static readonly string ME = "ME"; // Democracy
        public static readonly string MI = "MI"; // Environment
        public static readonly string NR = "NR"; // National accounts
        public static readonly string NV = "NV"; // Business activities
        public static readonly string OE = "OE"; // Public finances
        public static readonly string PR = "PR"; // Prices and Consumption
        public static readonly string TK = "TK"; // Transport and communications
        public static readonly string UF = "UF"; // Education and research
        

        private readonly IHttpClientFactory _httpClientFactory;

        public SCBHandler(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SCBTemplateDTO> GetSCBRegionAndGenderTemplate()
        {

            var httpClient = _httpClientFactory.CreateClient("SCB");

            using var response = await httpClient.GetAsync( $"{BE}/BE0101/BE0101H/FoddaK");

            if (response.IsSuccessStatusCode)
            {

                try
                {
                    return await response.Content.ReadFromJsonAsync<SCBTemplateDTO>();
                }
                catch (NotSupportedException e) 
                {
                    // log "Content type not valid"
                    throw e;
                }
                catch (JsonException e) // Invalid JSON
                {
                    // log "Invalid JSON"
                    throw e;
                }
            }
            return null;
        }
    }
}
