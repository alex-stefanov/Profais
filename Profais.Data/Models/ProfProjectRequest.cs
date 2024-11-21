using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Profais.Common.Enums;

namespace Profais.Data.Models;

public class ProfProjectRequest
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public string ClientId { get; set; } = null!;

    [ForeignKey(nameof(ClientId))]
    public virtual ProfUser Client { get; set; } = null!;

    [Required]
    public string ClientNumber { get; set; } = null!;

    [Required]
    public RequestStatus Status { get; set; }
}
