#region Usings

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

using Profais.Data.Models;

#endregion

namespace Profais.Areas.Identity.Pages.Account
{
    public class ProfileModel(
        UserManager<ProfUser> userManager,
        ILogger<ProfileModel> logger)
        : PageModel
    {
        [BindProperty]
        public ProfUser CurrentUser { get; set; } = null!;

        public List<string> Roles { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            string userId = userManager
                .GetUserId(User)!;

            ProfUser? user = await userManager
                .FindByIdAsync(userId);

            if(user is null)
            {
                logger.LogError("No user found");
                ViewData["ErrorMessage"] = "User not found";

                return NotFound();
            }

            CurrentUser = user;

            Roles.AddRange(await userManager
                .GetRolesAsync(user));

            return Page();
        }
    }
}
