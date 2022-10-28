using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Titan.Enums;

namespace Titan.Models.InheritModels
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(100, ErrorMessage = "Max Is 100"), MinLength(2, ErrorMessage = "Min Is 2")]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "Max Is 100"), MinLength(2, ErrorMessage = "Min Is 2")]
        public string? FatherName { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "Max Is 100"), MinLength(2, ErrorMessage = "Min Is 2")]
        public string? GrandFatherName { get; set; } = string.Empty;

        [Required, MaxLength(100, ErrorMessage = "Max Is 100"), MinLength(2, ErrorMessage = "Min Is 2")]
        public string LastName { get; set; } = string.Empty;

        public byte[]? ProfilePicture { get; set; }

        [EmailAddress]
        public string? OfficeEmail { get; set; } = string.Empty;

        [Phone]
        public string? OfficePhoneNumber { get; set; } = string.Empty;

        public Gender Gender { get; set; } = Gender.Male;
        public DateTime? DateOfBirth { get; set; }

        public double? FixedSalary { get; set; } = 0;
        public DateTime JoindDate { get; set; } = DateTime.Now;
    }
}
