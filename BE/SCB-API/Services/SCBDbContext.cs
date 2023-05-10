using Microsoft.EntityFrameworkCore;
using SCB_API.Models;

namespace SCB_API.Services
{
    public class SCBDbContext : DbContext
    {
        public SCBDbContext(DbContextOptions<SCBDbContext> options) : base(options)
        {

        }
        public DbSet<SCBModel> ScbModels { get; set; }
        public DbSet<BornStatistic> BornStatistics { get; set; }
    }
}



