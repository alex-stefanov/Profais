﻿using System.ComponentModel.DataAnnotations;
using Profais.Services.ViewModels.Message;
using Profais.Services.ViewModels.Task;
using static Profais.Common.Constants.ProjectConstants;

namespace Profais.Services.ViewModels.Project;

public class ProjectViewModel
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "Title must be between {2} and {1} characters.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Absolute address is required.")]
    [StringLength(AbsoluteAddressMaxLength, MinimumLength = AbsoluteAddressMinLength, ErrorMessage = "Absolute Address must be between {2} and {1} characters.")]
    public string AbsoluteAddress { get; set; } = null!;

    [Required(ErrorMessage = "Completion status is required.")]
    public bool IsCompleted { get; set; }

    [StringLength(SchemeMaxLength, MinimumLength = SchemeMinLength, ErrorMessage = "Scheme must be between {2} and {1} characters.")]
    public string? Scheme { get; set; }

    public virtual IEnumerable<MessageViewModel> Messages { get; set; }
        = new HashSet<MessageViewModel>();

    public virtual IEnumerable<TaskViewModel> Tasks { get; set; }
        = new HashSet<TaskViewModel>();
}