namespace Task_Application.Common.Settings;

public sealed class RefreshTokenSettings
{
    public const string SectionName = "RefreshTokenSettings";

    public int SessionExpirationDays { get; set; } = 1;
    public int PersistentExpirationDays { get; set; } = 7;
}
