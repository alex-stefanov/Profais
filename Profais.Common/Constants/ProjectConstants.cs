namespace Profais.Common.Constants;

/// <summary>
/// Contains constants related to project titles, absolute addresses, and schemes, such as maximum and minimum lengths.
/// </summary>
public static class ProjectConstants
{
    /// <summary>
    /// The maximum allowed length for a project title.
    /// </summary>
    public const int TitleMaxLength = 60;

    /// <summary>
    /// The minimum required length for a project title.
    /// </summary>
    public const int TitleMinLength = 3;

    /// <summary>
    /// The maximum allowed length for a project's absolute address.
    /// </summary>
    public const int AbsoluteAddressMaxLength = 250;

    /// <summary>
    /// The minimum required length for a project's absolute address.
    /// </summary>
    public const int AbsoluteAddressMinLength = 20;

    /// <summary>
    /// The maximum allowed length for a project's scheme.
    /// </summary>
    public const int SchemeMaxLength = 200;

    /// <summary>
    /// The minimum required length for a project's scheme.
    /// </summary>
    public const int SchemeMinLength = 10;
}