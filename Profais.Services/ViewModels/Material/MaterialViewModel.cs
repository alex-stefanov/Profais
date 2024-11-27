using Profais.Common.Enums;

namespace Profais.Services.ViewModels.Material;

public class MaterialViewModel
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required UsedFor UsedFor { get; set; }
}
