using System.ComponentModel.DataAnnotations;
using Profais.Services.ViewModels.Message;

namespace Profais.Services.ViewModels.Message;

public class PaginatedMessagesViewModel
{
    [Required]
    public required IEnumerable<MessageViewModel> Messages { get; set; }

    [Required]
    public int ProjectId { get; set; }

    [Required]
    public int CurrentPage { get; set; }

    [Required]
    public int TotalPages { get; set; }
}
