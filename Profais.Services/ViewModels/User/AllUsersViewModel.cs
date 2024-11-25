﻿namespace Profais.Services.ViewModels.User;

public class AllUsersViewModel
{
    public string Id { get; set; } = null!;

    public string? Email { get; set; }

    public IEnumerable<string> Roles { get; set; } = null!;
}
