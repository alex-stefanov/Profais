namespace Profais.Common.Constants;

/// <summary>
/// Contains constants related to project request titles, descriptions, and numbers, such as maximum and minimum lengths.
/// </summary>
public static class ProjectRequestConstants
{
    /// <summary>
    /// The maximum allowed length for a project request title.
    /// </summary>
    public const int TitleMaxLength = 30;

    /// <summary>
    /// The minimum required length for a project request title.
    /// </summary>
    public const int TitleMinLength = 3;

    /// <summary>
    /// The maximum allowed length for a project request description.
    /// </summary>
    public const int DescriptionMaxLength = 250;

    /// <summary>
    /// The minimum required length for a project request description.
    /// </summary>
    public const int DescriptionMinLength = 3;

    /// <summary>
    /// The maximum allowed length for a project request number.
    /// </summary>
    public const int NumberMaxLength = 15;

    /// <summary>
    /// The minimum required length for a project request number.
    /// </summary>
    public const int NumberMinLength = 10;
}
