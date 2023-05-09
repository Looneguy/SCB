using SCB_API.Models;

namespace SCB_API.Services
{
    public class Database
    {
        private readonly SCBDbContext _ctx;

        public Database(SCBDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> SeedAsync()
        {

            try
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

                var scbTable = new SCBTable()
                {
                    FetchedAt = DateTime.UtcNow,
                    RegionCode = "1407",
                    RegionName = "Öckerö",
                };


                _ctx.BornStatistics.Add(bornStatistic);
                _ctx.ScbTables.Add(scbTable);

                _ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                // Todo Add logger, SeriLog? 
                return false;
            }
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
