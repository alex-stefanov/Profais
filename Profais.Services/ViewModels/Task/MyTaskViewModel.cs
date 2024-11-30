using Profais.Services.ViewModels.Material;
using Profais.Services.ViewModels.Worker;

namespace Profais.Services.ViewModels.Task;

public class MyTaskViewModel
{
    public required int Id { get; set; }

    public required string UserId {  get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required int ProjectId { get; set; }

    public required bool IsVoted {  get; set; }

    public virtual IEnumerable<MaterialViewModel> Materials { get; set; }
        = new HashSet<MaterialViewModel>();

    public virtual IEnumerable<UserViewModel> Users { get; set; }
        = new HashSet<UserViewModel>();
}