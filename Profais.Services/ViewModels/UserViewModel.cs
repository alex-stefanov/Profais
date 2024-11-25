using System.ComponentModel.DataAnnotations;
using static Profais.Common.Constants.UserConstants;
namespace Profais.Services.ViewModels;

public class UserViewModel
{
    [Required]
    public required string Id { get; set; }

    [Required(ErrorMessage = "User's first name is required.")]
    [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength, ErrorMessage = "User's first name must be between {2} and {1} characters.")]
    public required string UserFirstName { get; set; }

    [Required(ErrorMessage = "User's last name is required.")]
    [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength, ErrorMessage = "User's last name must be between {2} and {1} characters.")]
    public required string UserLastName { get; set; }
}