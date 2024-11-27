using System.ComponentModel.DataAnnotations;
using Profais.Common.Enums;
using static Profais.Common.Constants.UserRequestConstants;

namespace Profais.Services.ViewModels.WorkerRequest;

public class MakeWorkerRequestViewModel
{
    [Required(ErrorMessage = "User id is required.")]
    public required string UserId { get; set; }

    [Required(ErrorMessage = "First Name is required.")]
    [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength, ErrorMessage = "First Name must be between {2} and {1} characters.")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required.")]
    [StringLength(LastNameMaxLength, MinimumLength = FirstNameMinLength, ErrorMessage = "Last Name must be between {2} and {1} characters.")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Profix Id is required.")]
    public required string ProfixId { get; set; }
}