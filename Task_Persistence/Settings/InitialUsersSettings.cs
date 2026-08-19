namespace Task_Persistence.Settings;

public sealed class InitialUsersSettings
{
    public const string SectionName = "InitialUsers";

    public InitialUserSettings Admin { get; init; } = new();
    public InitialUserSettings Demo { get; init; } = new();
}
