namespace Profais.Services.ViewModels.Penalty;

public class UserPenaltyViewModel
{
    public required IEnumerable<UserForPenaltyViewModel> Users { get; set; }

    public required IEnumerable<PenaltyViewModel> Penalties { get; set; }

    public int? SelectedPenaltyId { get; set; }

    public string? SelectedUserId { get; set; }
}
