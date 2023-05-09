using System.ComponentModel.DataAnnotations;

namespace SCB_API.Models
{
    // this Table is only to know which RegionName corresponds to which RegionCode
    // and Which code is for which sex.
    public class SCBTable
    {
        public int Id { get; set; }
        public DateTime FetchedAt { get; set; }
        public string? RegionCode { get; set; }
        public string? RegionName { get; set; }
        public string? SexCode { get; set; }
        public string? Sex { get; set; }
    }
}
