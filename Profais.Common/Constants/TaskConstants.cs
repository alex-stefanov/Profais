namespace Profais.Common.Constants;

/// <summary>
/// Contains constants related to task titles and descriptions, such as maximum and minimum lengths.
/// </summary>
public static class TaskConstants
{
    /// <summary>
    /// The maximum allowed length for a task title.
    /// </summary>
    public const int TitleMaxLength = 35;

    /// <summary>
    /// The minimum required length for a task title.
    /// </summary>
    public const int TitleMinLength = 4;

    /// <summary>
    /// The maximum allowed length for a task description.
    /// </summary>
    public const int DescriptionMaxLength = 500;

    /// <summary>
    /// The minimum required length for a task description.
    /// </summary>
    public const int DescriptionMinLength = 5;
}
