#region Usings

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

using static Profais.Common.Constants.UserConstants;

#endregion

namespace Profais.Data.Models;

/// <summary>
/// Represents a user in the system, extending from IdentityUser for authentication and authorization purposes.
/// </summary>
public class ProfUser
    : IdentityUser
{
    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    [Required]
    [PersonalData]
    [MaxLength(FirstNameMaxLength)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    [Required]
    [PersonalData]
    [MaxLength(LastNameMaxLength)]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Navigation property for the penalties associated with the user.
    /// </summary>
    public virtual ICollection<ProfUserPenalty> UserPenalties { get; }
        = new HashSet<ProfUserPenalty>();

    /// <summary>
    /// Navigation property for the project requests created by the user.
    /// </summary>
    public virtual ICollection<ProfProjectRequest> ProjectRequests { get; }
        = new HashSet<ProfProjectRequest>();

    /// <summary>
    /// Navigation property for the tasks assigned to the user.
    /// </summary>
    public virtual ICollection<ProfUserTask> UserTasks { get; }
        = new HashSet<ProfUserTask>();
}