using System.ComponentModel.DataAnnotations;

namespace SCB_API.Models.RequestModels
{
    public class BornRequestDTO
    {
        [Required]
        public string Region { get; set; } = string.Empty;
        public string? Year { get; set; } = null;
        public string? Gender { get; set; } = null;
    }
}
