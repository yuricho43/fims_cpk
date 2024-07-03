using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using FimsCPK.Entities;

namespace FimsCPK.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<AspNetUser> _signInManager;

        public LoginModel(SignInManager<AspNetUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public void OnGet()
        {
            ReturnUrl = Url.Content("~/initialize");
        }

        public async Task<ActionResult> OnPostAsync()
        {
            ReturnUrl = Url.Content("~/initialize");
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync
                    (Input.Name, Input.Password, false, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return LocalRedirect(ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Wrong Password or Username! Try again!");
                }
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

        }
    }
}
