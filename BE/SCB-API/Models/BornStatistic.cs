using System.ComponentModel.DataAnnotations;

namespace SCB_API.Models
{
    public class BornStatistic
    {
        public int Id { get; set; }
        public DateTime FetchedAt { get; set; } = new DateTime().Date;
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string Year { get; set; }
        public string Sex { get; set; }
        public int Amount { get; set; }
    }
}
