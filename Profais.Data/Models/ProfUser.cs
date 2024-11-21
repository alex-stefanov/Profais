using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Profais.Data.Models;

public class ProfUser
    : IdentityUser
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    public virtual ICollection<Message> Messages
        => new HashSet<Message>();

    public virtual ICollection<ProfUserPenalty> UserPenalties
        => new HashSet<ProfUserPenalty>();

    public virtual ICollection<ProfProjectRequest> ProjectRequests
        => new HashSet<ProfProjectRequest>();

    public virtual ICollection<ProfUserTask> UserTasks
        => new HashSet<ProfUserTask>();
}
