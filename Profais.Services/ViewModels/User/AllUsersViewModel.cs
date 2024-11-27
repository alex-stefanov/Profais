namespace Profais.Services.ViewModels.User;

public class AllUsersViewModel
{
	public required string Id { get; set; }

    public string? Email { get; set; }

    public IEnumerable<string> Roles { get; set; }
        = new HashSet<string>();
}
