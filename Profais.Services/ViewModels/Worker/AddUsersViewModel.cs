namespace Profais.Services.ViewModels.Worker;

public class AddUsersViewModel
{
    public required string Id { get; set; }

    public required string UserFirstName { get; set; }

    public required string UserLastName { get; set; }

    public required int TaskId { get; set; }
}