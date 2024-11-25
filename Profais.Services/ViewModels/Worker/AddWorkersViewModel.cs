using System.ComponentModel.DataAnnotations;

namespace Profais.Services.ViewModels.Worker;

public class AddWorkersViewModel
{
	[Required]
	public required int TaskId { get; set; }

	[Required]
	public required IEnumerable<UserViewModel> Users { get; set; }

	public IEnumerable<int> SelectedWorkerIds { get; set; } = new List<int>();

	[Required]
	public required int CurrentPage { get; set; }

	[Required]
	public required int TotalPages { get; set; }
}