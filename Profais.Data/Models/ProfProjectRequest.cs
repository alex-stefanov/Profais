using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Profais.Common.Enums;
using static Profais.Common.Constants.ProjectRequestConstants;

namespace Profais.Data.Models;

/// <summary>
/// Represents a project request in the system, associated with a client.
/// </summary>
public class ProfProjectRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the project request.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the project request.
    /// </summary>
    [Required]
    [MaxLength(TitleMaxLength)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the project request.
    /// </summary>
    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets the client ID associated with the project request.
    /// </summary>
    [Required]
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// Navigation property for the client associated with the project request.
    /// </summary>
    [ForeignKey(nameof(ClientId))]
    public virtual ProfUser Client { get; set; } = null!;

    /// <summary>
    /// Gets or sets the client number associated with the project request.
    /// </summary>
    [Required]
    [MaxLength(NumberMaxLength)]
    public string ClientNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets the status of the project request.
    /// </summary>
    [Required]
    public RequestStatus Status { get; set; }
}
