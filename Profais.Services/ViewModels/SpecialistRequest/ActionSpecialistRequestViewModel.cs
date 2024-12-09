﻿#region Usings

using System.ComponentModel.DataAnnotations;

#endregion

namespace Profais.Services.ViewModels.SpecialistRequest;

public class ActionSpecialistRequestViewModel
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "User id is required.")]
    public required string UserId { get; set; }
}
