namespace Profais.Common.Constants;

/// <summary>
/// Contains constants related to user request names, such as maximum and minimum lengths for first and last names.
/// </summary>
public static class UserRequestConstants
{
    /// <summary>
    /// The maximum allowed length for a user's first name in a request.
    /// </summary>
    public const int FirstNameMaxLength = 60;

    /// <summary>
    /// The minimum required length for a user's first name in a request.
    /// </summary>
    public const int FirstNameMinLength = 2;

    /// <summary>
    /// The maximum allowed length for a user's last name in a request.
    /// </summary>
    public const int LastNameMaxLength = 60;

    /// <summary>
    /// The minimum required length for a user's last name in a request.
    /// </summary>
    public const int LastNameMinLength = 2;
}
