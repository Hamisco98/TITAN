namespace Titan.Areas.Identity.ViewModels
{
    public class RoleViewModel
    {
        //This view Model used for getting all roles that assgined to a spesific user
        public string? RoleID { get; set; }
        public string? RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
