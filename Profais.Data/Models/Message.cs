using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Profais.Common.Constants.MessageConstants;

namespace Profais.Data.Models;

/// <summary>
/// Represents a message exchanged in the context of a project and a client.
/// </summary>
public class Message
{
    /// <summary>
    /// Gets or sets the ID of the associated project.
    /// </summary>
    [Required]
    public int ProjectId { get; set; }

    /// <summary>
    /// Navigation property for the associated project.
    /// </summary>
    [ForeignKey(nameof(ProjectId))]
    public virtual ProfProject Project { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID of the client associated with the message.
    /// </summary>
    [Required]
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// Navigation property for the client associated with the message.
    /// </summary>
    [ForeignKey(nameof(ClientId))]
    public ProfUser Client { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the message.
    /// </summary>
    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; } = null!;
}