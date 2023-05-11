using Newtonsoft.Json;
using SCB_API.Enums;
using SCB_API.Models;
using SCB_API.Models.ResponseModels;
using System.Net;

namespace SCB_API.Services
{
    public class SCBHandler
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SCBHandler(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SCBTemplateDTO> GetSCBRegionAndGenderTemplate()
        {

            var httpClient = _httpClientFactory.CreateClient("SCB");

            using var response = await httpClient.GetAsync($"{SCBSubcategories.BE}/BE0101/BE0101H/FoddaK");

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
                catch (JsonException e)
                {
                    // log "Invalid JSON"
                    throw e;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets statistics for everyone born in sweden
        /// between 2016-2020.
        /// Includes gender, amount born per region
        /// </summary>
        /// <returns>Null if fetching data failed</returns>
        public async Task<SCBResponse<BornStatisticDto>> GetBornStatisticsFromSCB(string[] years)
        {
            var httpClient = _httpClientFactory.CreateClient("SCB");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            
            var queries = new Query[]
                {
                    new Query()
                    {
                        Code = SCBCodes.Region, Selection = new Selection()
                        {
                            Filter = SCBFilters.All,
                            Values = new[] {"*"}
                        }
                    },
                    new Query()
                    {
                        Code = SCBCodes.Gender, Selection= new Selection()
                        {
                            Filter = SCBFilters.All,
                            Values = new[] {"*"}
                        }
                    },
                    new Query()
                    {
                        Code = SCBCodes.Time, Selection= new Selection()
                        {
                            Filter= SCBFilters.Item,
                            Values = years
                        }
                    }
                };
            var body = createSCBBody(queries, SCBResponseFormats.Json);

            var response = await httpClient.PostAsJsonAsync($"{SCBSubcategories.BE}/BE0101/BE0101H/FoddaK", body);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var result = await response.Content.ReadFromJsonAsync<BornStatisticDto>();
                    return new SCBResponse<BornStatisticDto>(result);
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
            string errorMessage = $"Failed fetching statistics | Error - {response.ReasonPhrase} | Status code - {response.StatusCode} |";
            return new SCBResponse<BornStatisticDto>(errorMessage);
        }

        /// <summary>
        /// Creates a request body according to SCB's Api - 
        /// (<paramref name="responseFormat"/>) Accepted response-types are "px","csv", "json", "xlsx", "json-stat", "json-stat2", "sdmx" 
        /// </summary>
        /// <param name="queries"></param>
        /// <param name="responseFormat">Accepted response-types are "px","csv", "json", "xlsx", "json-stat", "json-stat2", "sdmx"</param>
        /// <returns></returns>
        private SCBBody createSCBBody(Query[] queries, string responseFormat) 
        {
            return new SCBBody() { Query= queries, Response = new Response() { Format = responseFormat} };
        }

    }
}
