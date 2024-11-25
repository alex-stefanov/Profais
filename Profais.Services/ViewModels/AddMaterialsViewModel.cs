﻿using Profais.Common.Enums;
using Profais.Services.ViewModels;

public class AddMaterialsViewModel
{
    public int TaskId { get; set; }

    public IEnumerable<MaterialViewModel> Materials { get; set; } = new List<MaterialViewModel>();

    public List<UsedFor> UsedForEnumValues { get; set; } = new List<UsedFor>();

    public List<int> SelectedMaterials { get; set; } = new List<int>();
}
