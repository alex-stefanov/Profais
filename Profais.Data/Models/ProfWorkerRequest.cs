namespace Profais.Data.Models;

/// <summary>
/// Represents a request made by a worker, inheriting from <see cref="ProfUserRequest"/>.
/// This class allows additional worker-specific request handling.
/// </summary>
public class ProfWorkerRequest
    : ProfUserRequest { }