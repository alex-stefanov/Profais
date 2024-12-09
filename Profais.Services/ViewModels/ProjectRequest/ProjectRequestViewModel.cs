#region Usings

using Profais.Common.Enums;

#endregion

namespace Profais.Services.ViewModels.ProjectRequest;

public class ProjectRequestViewModel
{
    public required int Id { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required string ClientName { get; set; }

    public required string ClientNumber { get; set; }

    public required RequestStatus Status { get; set; }
}
