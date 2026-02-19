using System.ComponentModel.DataAnnotations;

namespace CompanyProject.Data.Models
{
    public class SystemRule
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Code { get; set; } = string.Empty;

        public bool IsEnabled { get; set; }

        public string? ValueJson { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(450)]
        public string? UpdatedBy { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
