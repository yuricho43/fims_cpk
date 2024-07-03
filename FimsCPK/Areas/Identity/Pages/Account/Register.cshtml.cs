using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Telerik.SvgIcons;
using FimsCPK.Entities;

namespace FimsCPK.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly SignInManager<AspNetUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(UserManager<AspNetUser> userManager,
            SignInManager<AspNetUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public void OnGet()
        {
            ReturnUrl = Url.Content("~/");
        }

        public async Task<ActionResult> OnPostAsync()
        {
            ReturnUrl = Url.Content("~/");
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByNameAsync(Input.Name);
                if (userExists != null)
                {
                    ModelState.AddModelError(string.Empty, "This user name은 이미 있음. 다른 이름으로 시도하쇼!");
                    return Page();
                }
                var identity = new AspNetUser { UserName = Input.Name };
                identity.EnglishName = Input.CompanyName;
                identity.HangulName = Input.Department;
                identity.Email = Input.Email;
                var result = await _userManager.CreateAsync(identity, Input.Password);

                // Roles
                var role = new IdentityRole(Input.Role);

                var roleExists = await _roleManager.FindByNameAsync(role.Name);
                if (roleExists == null)
                {
                    var addRoleResults = await _roleManager.CreateAsync(role);
                    if (!addRoleResults.Succeeded)
                    {
                        return Page();
                    }
                }

                var addUserRoleResult = await _userManager.AddToRoleAsync(identity, Input.Role);
                if (result.Succeeded && addUserRoleResult.Succeeded)
                {
                    await _signInManager.SignInAsync(identity, isPersistent: false);
                    return LocalRedirect(ReturnUrl);
                }
                await _userManager.AddClaimAsync(identity, new Claim("EnglishName", identity.EnglishName));
            }
            return Page();

        }
        public class InputModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            public string Role { get; set; }

            [Required]
            public string CompanyName { get; set; }

            [Required]
            public string Department { get; set; }

            [Required]
            public string Email { get; set; }


        }
    }
}
