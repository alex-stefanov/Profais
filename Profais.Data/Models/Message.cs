using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profais.Data.Models;

public class Message
{
    [Required]
    public int ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public virtual ProfProject Project { get; set; } = null!;

    [Required]
    public string ClientId { get; set; } = null!;

    [ForeignKey(nameof(ClientId))]
    public ProfUser Client { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;
}