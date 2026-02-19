using System.ComponentModel.DataAnnotations;

namespace CompanyProject.ViewModels.Rules
{
    public class SystemRuleEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = string.Empty;

        public bool IsEnabled { get; set; }

        public string? ValueJson { get; set; }

        public string? RowVersionBase64 { get; set; }
    }
}
