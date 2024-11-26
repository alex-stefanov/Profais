using System.ComponentModel.DataAnnotations;

namespace Profais.Services.ViewModels.Penalty;

public class FullCollectionPenaltyViewModel
{
	/// <summary>
	/// Gets or sets the unique identifier for the penalty.
	/// </summary>
	[Required]
	public required int Id { get; set; }

	/// <summary>
	/// Gets or sets the title of the penalty.
	/// </summary>
	[Required]
	public required string Title { get; set; }

	[Required]
	public required string UserName { get; set; }
	[Required]
	public required string UserId { get; set; }
}