﻿#region Usings

using Profais.Services.ViewModels.Task;
using Profais.Services.ViewModels.Worker;

#endregion

namespace Profais.Services.ViewModels.Project;

public class ProjectViewModel
{
    public required int Id { get; set; }

    public required string Title { get; set; }

    public required string AbsoluteAddress { get; set; }

    public required bool IsCompleted { get; set; }

    public string? Scheme { get; set; }

    public virtual IEnumerable<UserViewModel> Contributers { get; set; } = [];

    public virtual IEnumerable<TaskViewModel> Tasks { get; set; } = [];
}