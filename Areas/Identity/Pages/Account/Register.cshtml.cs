// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuthTest.Model;
using AuthTest.Data;

namespace AuthTest.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        public List<SelectListItem> Roles { get; set; }

        [RequiredIfRole("Member")]
        public List<SelectListItem> MemberExperienceLevels { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(250, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 250 characters.")]
            public string Fullname { get; set; }

            public string ProfilePictureURL { get; set; }

            [Required]
            public string Role { get; set; }

            // Manager
            [RequiredIfRole("Manager")]
            public string Department { get; set; }

            // Member
            [RequiredIfRole("TeamMember")]
            public string JobTitle { get; set; }
            [RequiredIfRole("TeamMember")]
            public string SkillSet { get; set; }
            [RequiredIfRole("TeamMember")]
            public int MemberExperienceLevelId { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            Roles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();

            MemberExperienceLevels = _context.MemberExperienceLevels
                .Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.LevelName
                }).ToList();

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                // Optional: Assign custom fields
                user.FullName = Input.Fullname;
                user.ProfilePictureUrl = Input.ProfilePictureURL;
                user.Role = Input.Role;

                if(Input.Role == "Manager")
                {
                    var managerProfile = new ManagerProfile
                    {
                        Department = Input.Department
                    };

                    _context.ManagerProfiles.Add(managerProfile);
                    await _context.SaveChangesAsync();
                    // Here you would typically save the managerProfile to the database and get its Id
                    user.ManagerProfileId = managerProfile.Id;
                }
                else if(Input.Role == "TeamMember")
                {
                    var memberProfile = new MemberProfile
                    {
                        JobTitle = Input.JobTitle,
                        SkillSet = Input.SkillSet,
                        MemberExperienceLevelId = Input.MemberExperienceLevelId
                        // For simplicity, AssignedProjectsId is left empty
                    };

                    _context.MemberProfiles.Add(memberProfile);
                    await _context.SaveChangesAsync();
                    // Here you would typically save the memberProfile to the database and get its Id
                    user.MemberProfileId = memberProfile.Id;
                }

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Ensure role exists and assign
                    // if (!await _roleManager.RoleExistsAsync(Input.Role))
                    //     await _roleManager.CreateAsync(new IdentityRole(Input.Role));

                    await _userManager.AddToRoleAsync(user, Input.Role);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    // return LocalRedirect(returnUrl);
                    return RedirectToPage("/Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}