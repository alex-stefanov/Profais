using Profais.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profais.Data.Models;

[NotMapped]
public class ProfUserRequest
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string ProfixId { get; set; } = null!;

    [Required]
    public RequestStatus Status { get; set; }
}
