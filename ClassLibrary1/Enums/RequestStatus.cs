using System.ComponentModel.DataAnnotations;

namespace Profais.Common.Enums;

/// <summary>
/// Represents the possible statuses for a request.
/// </summary>
public enum RequestStatus
{
    /// <summary>
    /// The request is pending and has not yet been approved or declined.
    /// </summary>
    [Display(Name = "Pending")]
    Pending = 0,

    /// <summary>
    /// The request has been approved.
    /// </summary>
    [Display(Name = "Approved")]
    Approved = 1,

    /// <summary>
    /// The request has been declined.
    /// </summary>
    [Display(Name = "Declined")]
    Declined = 2,
}
