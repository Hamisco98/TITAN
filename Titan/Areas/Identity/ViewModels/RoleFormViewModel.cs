using System.ComponentModel.DataAnnotations;

namespace Titan.Areas.Identity.ViewModels
{
    public class RoleFormViewModel
    {
        [Required]
        [StringLength(256)]
        public string RoleName { get; set; } = string.Empty;
    }
}
