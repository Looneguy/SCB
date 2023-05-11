using SCB_API.Models;
using SCB_API.Enums;
using Microsoft.EntityFrameworkCore;

namespace SCB_API.Services
{
    public class Database
    {
        private readonly SCBDbContext _ctx;

        public Database(SCBDbContext ctx)
        {
            _ctx = ctx;
        }

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

        public async Task RecreateAndSeedAsync()
        {
            await _ctx.Database.EnsureDeletedAsync();
            await _ctx.Database.EnsureCreatedAsync();
            await SeedAsync();
        }

        public async Task<bool> Recreate()
        {
            await _ctx.Database.EnsureDeletedAsync();
            var isCreated = await _ctx.Database.EnsureCreatedAsync();
            return isCreated;
        }

        public async Task<bool> CreateIfNotExist()
        {
            var isCreated = await _ctx.Database.EnsureCreatedAsync();
            return isCreated;
        }

        public async Task CreateAndSeedIfNotExist()
        {
            bool didCreateDatabase = await _ctx.Database.EnsureCreatedAsync();
            if (didCreateDatabase)
            {
                await SeedAsync();
            }
        }

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
