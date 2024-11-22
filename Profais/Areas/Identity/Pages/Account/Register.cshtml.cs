using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Profais.Data.Models;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Areas.Identity.Pages.Account;

public class RegisterModel(
    UserManager<ProfUser> userManager,
    IUserStore<ProfUser> userStore,
    SignInManager<ProfUser> signInManager,
    ILogger<RegisterModel> logger)
    : PageModel
{
    private IUserEmailStore<ProfUser> emailStore
        => GetEmailStore();

    [BindProperty]
    public InputModel Input { get; set; } = null!;

    public string? ReturnUrl { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; } = [];

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(PasswordMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = PasswordMinLength)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        [StringLength(FirstNameMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = FirstNameMinLength)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = LastNameMinLength)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;
    }

    public async Task OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
        ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        if (ModelState.IsValid)
        {
            var user = CreateUser();

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;

            await userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            var result = await userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                logger.LogInformation("User created a new account with password.");

                await signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return Page();
    }

    private ProfUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ProfUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ProfUser)}'. " +
                $"Ensure that '{nameof(ProfUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    private IUserEmailStore<ProfUser> GetEmailStore()
    {
        if (!userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ProfUser>)userStore;
    }
}
