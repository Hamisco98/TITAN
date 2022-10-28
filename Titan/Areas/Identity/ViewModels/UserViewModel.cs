namespace Titan.Areas.Identity.ViewModels
{
    public class UserViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[] ImagePicture { get; set; }
        public IEnumerable<string> Roles { get; set; }


    }
}
