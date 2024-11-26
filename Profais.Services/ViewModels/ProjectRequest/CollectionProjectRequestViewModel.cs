using System.ComponentModel.DataAnnotations;
using static Profais.Common.Constants.ProjectRequestConstants;

namespace Profais.Services.ViewModels.ProjectRequest;

public class CollectionProjectRequestViewModel
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "Title must be between {2} and {1} characters.")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Client Name is required.")]
    public required string ClientName { get; set; }
}