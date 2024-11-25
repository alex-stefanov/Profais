using Profais.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Profais.Common.Constants.UserRequestConstants;

namespace Profais.Data.Models;

/// <summary>
/// Represents a request made by a user, containing user details and request status.
/// This class is not mapped to a database table.
/// </summary>
[NotMapped]
public class ProfUserRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the user request.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the id of the user making the request.
    /// </summary>
    [Required]
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the first name of the user making the request.
    /// </summary>
    [Required]
    [MaxLength(FirstNameMaxLength)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the last name of the user making the request.
    /// </summary>
    [Required]
    [MaxLength(LastNameMaxLength)]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the prefix associated with the user.
    /// </summary>
    [Required]
    public string ProfixId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the current status of the request.
    /// </summary>
    [Required]
    public RequestStatus Status { get; set; }
}