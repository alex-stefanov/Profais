using System.ComponentModel.DataAnnotations;

namespace Profais.Services.ViewModels.WorkerRequest;

public class ActionWorkerRequestViewModel
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "User id is required.")]
    public required string UserId { get; set; }
}
