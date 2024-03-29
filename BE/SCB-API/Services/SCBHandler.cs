﻿using Newtonsoft.Json;
using SCB_API.Enums;
using SCB_API.Models;
using SCB_API.Models.ResponseModels;
using SCB_API.Tools.Helpers;

namespace SCB_API.Services
{
    public class SCBHandler
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DatabaseHandler _databaseHandler;
        private readonly SCBDbContext _ctx;

        public SCBHandler(IHttpClientFactory httpClientFactory, DatabaseHandler databaseHandler, SCBDbContext ctx) {
            _httpClientFactory = httpClientFactory;
            _databaseHandler = databaseHandler;
            _ctx = ctx;
        }

        /// <summary>
        /// Gets the template for region-code and region-name, also which gender-code is for which gender.
        /// </summary>
        /// <returns></returns>
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
        /// Includes genders, amount born per region
        /// </summary>
        /// <returns>Null if fetching data failed</returns>
        public async Task<SCBResponse<BornStatisticDto>> GetBornStatisticsFromSCBAsync(string[] years)
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
        /// Gets the statistics from people born in sweden, based on 
        /// <paramref name="year"/>,
        /// <paramref name="region"/>(region name), 
        /// <paramref name="gender"/>
        /// </summary>
        /// <param name="year"></param>
        /// <param name="region"></param>
        /// <param name="gender"></param> 
        /// <returns></returns>
        public SCBResponse<List<BornStatistic>> GetBornStatistic(string? region = "", string? year = "", string? gender = "")
        {
            var check = new Checkers();

            IQueryable<BornStatistic> query = _ctx.BornStatistics.AsQueryable<BornStatistic>();

            if (!check.IsNullOrUndefined(gender))
            {
                gender = gender.ToLower();
                query = query.Where(x => x.Gender == gender);
            };

            if (!check.IsNullOrUndefined(region))
            {
                region = FirstCharToUppercase(region);
                query = query.Where(x => x.RegionName == region);
            };


            if (!check.IsNullOrUndefined(year))
            {
                query = query.Where(x => x.Year== year);
            };

            var result = query.ToList();
            if(result.Count != 0)
            {
                return new SCBResponse<List<BornStatistic>>(result);
            }

            string errorMessage = ErrorBuilder(region, year, gender);
            return new SCBResponse<List<BornStatistic>>(errorMessage);
        }

        public SCBResponse<List<string>> GetRegions()
        {
            var templateList = _ctx.ScbModels.Where(m => m.RegionName != null).ToList();

            List<string> regions = new List<string>();
            if(templateList.Count != 0)
            {
                foreach (var template in templateList)
                {
                    regions.Add(template.RegionName);
                }

                return new SCBResponse<List<string>>(regions);
            }

            string errorMessage = "No regions where found in database";
            return new SCBResponse<List<string>>(errorMessage);
        }

        /// <summary>
        /// Creates a request-body according to SCB's Api - 
        /// (<paramref name="responseFormat"/>) Accepted response-types are "px","csv", "json", "xlsx", "json-stat", "json-stat2", "sdmx" 
        /// </summary>
        /// <param name="queries"></param>
        /// <param name="responseFormat">Accepted response-types are "px","csv", "json", "xlsx", "json-stat", "json-stat2", "sdmx"</param>
        /// <returns></returns>
        private SCBBody createSCBBody(Query[] queries, string responseFormat) 
        {
            return new SCBBody() { Query= queries, Response = new Response() { Format = responseFormat} };
        }

        /// <summary>
        /// Helper to capitalize first char in a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string FirstCharToUppercase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            return $"{input[0].ToString().ToUpper()}{input.Substring(1)}";
        }

        private string ErrorBuilder(string region, string year, string gender)
        {
            return $"With these filters: Region - '{region}', Year - '{year}', Gender - '{gender}'. No statistics could be found. Make sure years are one of 2016-2020 and check spellings";
        }
    }
}
