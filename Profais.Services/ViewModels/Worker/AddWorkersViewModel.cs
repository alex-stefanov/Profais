namespace Profais.Services.ViewModels.Worker;

public class AddWorkersViewModel
{
	public required int TaskId { get; set; }

	public required IEnumerable<UserViewModel> Users { get; set; }

	public IEnumerable<string> SelectedWorkerIds { get; set; } 
		= new HashSet<string>();

	public required int CurrentPage { get; set; }

	public required int TotalPages { get; set; }
}