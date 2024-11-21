using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profais.Data.Models;

public class ProfProject
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string AbsoluteAddress { get; set; } = null!;

    [Required]
    public int ProfProjectRequestId { get; set; }

    [ForeignKey(nameof(ProfProjectRequestId))]
    public virtual ProfProjectRequest ProfProjectRequest { get; set; } = null!;

    [Required]
    public bool IsCompleted { get; set; }

    public string? Scheme { get; set; }

    public virtual ICollection<Message> Messages
        => new HashSet<Message>();
}