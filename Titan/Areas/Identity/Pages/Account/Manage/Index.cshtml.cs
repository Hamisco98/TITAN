﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Titan.Enums;
using Titan.Models.InheritModels;

namespace Titan.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        
        public string Username { get; set; }

        
        [TempData]
        public string StatusMessage { get; set; }

        
        [BindProperty]
        public InputModel Input { get; set; }

        
        public class InputModel
        {
            [Phone]
            [Display(Name = "Personal Phone number")]
            public string PhoneNumber { get; set; }

            #region Application Model
            [Required, MaxLength(100, ErrorMessage = "Max Is 100"), MinLength(2, ErrorMessage = "Min Is 2")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; } = string.Empty;

            [MaxLength(100, ErrorMessage = "Max Is 100"), MinLength(2, ErrorMessage = "Min Is 2")]
            [Display(Name = "Father Name")]
            public string FatherName { get; set; } = string.Empty;

            [MaxLength(100, ErrorMessage = "Max Is 100"), MinLength(2, ErrorMessage = "Min Is 2")]
            [Display(Name = "Grand Father Name")]
            public string GrandFatherName { get; set; } = string.Empty;

            [Required, MaxLength(100, ErrorMessage = "Max Is 100"), MinLength(2, ErrorMessage = "Min Is 2")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; } = string.Empty;

            [Display(Name = "Profile Picture")]
            public byte[] ProfilePicture { get; set; }


            [Phone]
            [Display(Name = "Office Phone Number")]
            public string OfficePhoneNumber { get; set; } = string.Empty;

            public Gender Gender { get; set; } = Gender.Male;

            [Display(Name = "Date Of Birth")] 
            public DateTime? DateOfBirth { get; set; }

            [Display(Name = "Salary")] 
            public double? FixedSalary { get; set; } = 0;
            
            [Display(Name = "Joind Date")] 
            public DateTime JoindDate { get; set; } = DateTime.Now;
            #endregion
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FatherName = user.FatherName,
                GrandFatherName = user.GrandFatherName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                JoindDate = user.JoindDate,
                FixedSalary = user.FixedSalary,
                OfficePhoneNumber = user.OfficePhoneNumber,
                ProfilePicture = user.ProfilePicture
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            user.FirstName = Input.FirstName;
            user.FatherName = Input.FatherName;
            user.GrandFatherName = Input.GrandFatherName;
            user.LastName= Input.LastName;
            user.OfficePhoneNumber = Input.OfficePhoneNumber;
            user.DateOfBirth = Input.DateOfBirth;
            user.FixedSalary = Input.FixedSalary;
            user.Gender = Input.Gender;
            
            if(Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    user.ProfilePicture = dataStream.ToArray();
                }
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
