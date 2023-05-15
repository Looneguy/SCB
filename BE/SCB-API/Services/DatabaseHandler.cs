using SCB_API.Models;
using SCB_API.Enums;
using Microsoft.EntityFrameworkCore;

namespace SCB_API.Services
{
    public class DatabaseHandler
    {
        private readonly SCBDbContext _ctx;

        public DatabaseHandler(SCBDbContext ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// Gets the "template" from SCB, which regioncode corresponds to which regionname,
        /// which gendercode to which gender and puts it into DB.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public async Task FillDbWithRegionAndGenderTemplateAsync(SCBTemplateDTO template)
        {
            foreach (var section in template.Variables)
            {
                if (section.Code == SCBCodes.Region)
                {
                    string[] regionCodes = section.Values;
                    string[] regionNames = section.ValueTexts;
                    var regions = new List<SCBModel>();

                    var codeAndName = regionCodes.Zip(regionNames, (c, n) => new { Code = c, Name = n });
                    foreach(var region in codeAndName)
                    {
                        regions.Add(new SCBModel() { RegionCode = region.Code, RegionName = region.Name, FetchedAt = DateTime.UtcNow});
                    }
                    _ctx.ScbModels.AddRange(regions);
                    _ctx.SaveChanges();
                }
                if (section.Code == SCBCodes.Gender)
                {
                    string[] codes = section.Values;
                    string[] genderNames = section.ValueTexts;
                    var genders = new List<SCBModel>();

                    var codeAndGender = codes.Zip(genderNames, (c, s) => new { Code = c, Name = s });
                    foreach(var gender in codeAndGender)
                    {
                        genders.Add(new SCBModel() { GenderCode = gender.Code, Gender = gender.Name, FetchedAt = DateTime.UtcNow });
                    }
                    _ctx.ScbModels.AddRange(genders);
                    _ctx.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Seed datanase with testdata
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SeedAsync()
        {
            var bornStatistic = new BornStatistic()
            {
                FetchedAt = DateTime.UtcNow,
                RegionCode = "00",
                RegionName = "Riket",
                Year = "2016",
                Gender = "Women",
                Amount = "10000"
            };

            var scbModel = new SCBModel()
            {
                FetchedAt = DateTime.UtcNow,
                RegionCode = "1407",
                RegionName = "Öckerö",
            };


            _ctx.BornStatistics.Add(bornStatistic);
            _ctx.ScbModels.Add(scbModel);

            _ctx.SaveChanges();
            return true;
        }

        /// <summary>
        /// Recreates the database and seeds it with testdata.
        /// </summary>
        /// <returns></returns>
        public async Task RecreateAndSeedAsync()
        {
            await _ctx.Database.EnsureDeletedAsync();
            await _ctx.Database.EnsureCreatedAsync();
            await SeedAsync();
        }

        /// <summary>
        /// Recreates the database.
        /// </summary>
        /// <returns>True if database is created</returns>
        public async Task<bool> Recreate()
        {
            await _ctx.Database.EnsureDeletedAsync();
            var isCreated = await _ctx.Database.EnsureCreatedAsync();
            return isCreated;
        }

        /// <summary>
        /// Creates database if no one exists already.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateIfNotExist()
        {
            var isCreated = await _ctx.Database.EnsureCreatedAsync();
            return isCreated;
        }

        /// <summary>
        /// Creates data base if it does not exists, then seeds database with testdata
        /// </summary>
        /// <returns>True if database is created</returns>
        public async Task<bool> CreateAndSeedIfNotExist()
        {
            bool didCreateDatabase = await _ctx.Database.EnsureCreatedAsync();
            if (didCreateDatabase)
            {
                await SeedAsync();
            }
            return didCreateDatabase;
        }

        /// <summary>
        /// Fills table "BornStatistics" with live births by region, gender, year and amount. 
        /// The method gets which region name and gender it is from the other table called ScbModels.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task FillDbWithBornStatistics(BornStatisticDto values)
        {
            var newListOfBornStatistics = new List<BornStatistic>();
            foreach(var value in values.Data)
            {
                var regionCode = value.Key[0];
                var genderCode = value.Key[1];
                var year = value.Key[2];
                var amount = value.Values[0];

                var regionTemplate = await _ctx.ScbModels.FirstOrDefaultAsync(m => m.RegionCode == regionCode);
                var genderTemplate = await _ctx.ScbModels.FirstOrDefaultAsync(m => m.GenderCode == genderCode);

                var newStatisticData = new BornStatistic()
                {
                    RegionName = regionTemplate.RegionName,
                    RegionCode = regionTemplate.RegionCode,
                    Year = year,
                    Gender = genderTemplate.Gender,
                    Amount = amount,
                    FetchedAt = value.FetchedAt,
                };

                newListOfBornStatistics.Add(newStatisticData);
            }
            
            await _ctx.BornStatistics.AddRangeAsync(newListOfBornStatistics);
            await _ctx.SaveChangesAsync();
        }
    }
}
