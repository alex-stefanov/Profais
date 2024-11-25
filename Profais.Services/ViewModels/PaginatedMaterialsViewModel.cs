using System.ComponentModel.DataAnnotations;
using Profais.Common.Enums;
using Profais.Services.ViewModels;

public class PaginatedMaterialsViewModel
{
	[Required]
	public required List<MaterialViewModel> Materials { get; set; }

	[Required]
	public required int TotalPages { get; set; }

	[Required]
	public required int CurrentPage { get; set; }

	[Required]
	public required List<UsedFor> UsedForEnumValues { get; set; }

	[Required]
	public required int TaskId { get; set; }
}