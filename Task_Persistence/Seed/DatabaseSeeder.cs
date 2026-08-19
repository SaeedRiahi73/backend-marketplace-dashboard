using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Task_Application.Contracts.Interfaces.Security;
using Task_Domain.Entities;
using Task_Domain.Enums;
using Task_Persistence.Context;
using Task_Persistence.Settings;

namespace Task_Persistence.Seed;

public sealed class DatabaseSeeder
{
    private readonly TaskDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly InitialUsersSettings _settings;

    public DatabaseSeeder(
        TaskDbContext dbContext,
        IPasswordHasher passwordHasher,
        IOptions<InitialUsersSettings> options)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _settings = options.Value;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedUserAsync(
            _settings.Admin,
            UserRole.Admin,
            nameof(InitialUsersSettings.Admin),
            cancellationToken);

        await SeedUserAsync(
            _settings.Demo,
            UserRole.Demo,
            nameof(InitialUsersSettings.Demo),
            cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedUserAsync(
        InitialUserSettings settings,
        UserRole expectedRole,
        string settingsName,
        CancellationToken cancellationToken)
    {
        ValidateSettings(settings, settingsName);

        string username = settings.Username.Trim();
        string email = settings.Email.Trim().ToLowerInvariant();

        User? user = _dbContext.Users.Local
            .SingleOrDefault(x => x.Username == username);

        user ??= await _dbContext.Users
            .SingleOrDefaultAsync(x => x.Username == username, cancellationToken);

        if (user is null)
        {
            bool emailAlreadyExists = _dbContext.Users.Local.Any(x => x.Email == email)
                || await _dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken);

            if (emailAlreadyExists)
            {
                throw new InvalidOperationException(
                    $"The email configured for InitialUsers:{settingsName} is already in use.");
            }

            string passwordHash = _passwordHasher.GenerateHash(settings.Password);
            await _dbContext.Users.AddAsync(
                new User(username, email, passwordHash, expectedRole),
                cancellationToken);

            return;
        }

        if (user.Role != expectedRole)
        {
            throw new InvalidOperationException(
                $"The user configured for InitialUsers:{settingsName} has an unexpected role.");
        }

        if (!string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"The user configured for InitialUsers:{settingsName} has an unexpected email address.");
        }

        if (!_passwordHasher.VerifyPassword(settings.Password, user.PasswordHash))
        {
            user.ChangePassword(_passwordHasher.GenerateHash(settings.Password));
        }
    }

    private static void ValidateSettings(
        InitialUserSettings settings,
        string settingsName)
    {
        if (string.IsNullOrWhiteSpace(settings.Username))
        {
            throw new InvalidOperationException(
                $"InitialUsers:{settingsName}:Username is required.");
        }

        if (string.IsNullOrWhiteSpace(settings.Email))
        {
            throw new InvalidOperationException(
                $"InitialUsers:{settingsName}:Email is required.");
        }

        if (string.IsNullOrWhiteSpace(settings.Password))
        {
            throw new InvalidOperationException(
                $"InitialUsers:{settingsName}:Password is required.");
        }
    }
}
