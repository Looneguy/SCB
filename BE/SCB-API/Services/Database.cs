using SCB_API.Models;
using System.Runtime.CompilerServices;

namespace SCB_API.Services
{
    public class Database
    {
        private static readonly string Region = "Region";
        private static readonly string Gender = "Kon";

        private readonly SCBDbContext _ctx;

        public Database(SCBDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task FillDbWithRegionAndGenderTemplateAsync(SCBTemplateDTO template)
        {
            foreach (var section in template.Variables)
            {
                if (section.Code == Region)
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
                if (section.Code == Gender)
                {
                    string[] codes = section.Values;
                    string[] genderNames = section.ValueTexts;
                    var genders = new List<SCBModel>();

                    var codeAndGender = codes.Zip(genderNames, (c, s) => new { Code = c, Name = s });
                    foreach(var gender in codeAndGender)
                    {
                        genders.Add(new SCBModel() { GenderCode = gender.Code, Sex = gender.Name, FetchedAt = DateTime.UtcNow });
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
                Sex = "Women",
                Amount = 10000
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
    }
}
