using System.ComponentModel.DataAnnotations;
using Profais.Services.ViewModels;

public class AddWorkersViewModel
{
	[Required]
	public required int TaskId { get; set; }

	[Required]
	public required IEnumerable<UserViewModel> Users { get; set; }

	public IEnumerable<int> SelectedWorkerIds { get; set; }

	[Required]
	public required int CurrentPage { get; set; }

	[Required]
	public required int TotalPages { get; set; }
}