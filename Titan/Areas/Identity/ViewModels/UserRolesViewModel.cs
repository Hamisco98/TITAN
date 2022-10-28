namespace Titan.Areas.Identity.ViewModels
{
    public class UserRolesViewModel
    {
        public string? UserID { get; set; }
        public string? Username { get; set; }
        public List<RoleViewModel>? Roles { get; set; }
    }
}
