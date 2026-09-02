using System.Text.RegularExpressions;
using Task_Domain.Common;
using Task_Domain.Enums;

namespace Task_Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public string NormalizedUsername { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public int TokenVersion { get; private set; }
    public bool IsSystemUser { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string? Image { get; private set; }

    // سازنده بدون پارامتر برای EF
    private User() { }

    // سازنده اصلی
    public User(
        string username,
        string email,
        string passwordHash,
        UserRole role,
        bool isSystemUser = false,
        string? image = null)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username cannot be empty.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password cannot be empty.");

        // ولیدیشن اولیه ایمیل در لایه دمین
        ValidateEmail(email);

        Id = Guid.NewGuid();
        Username = username.Trim();
        NormalizedUsername = Username.ToLowerInvariant();
        Email = email.ToLowerInvariant().Trim();
        PasswordHash = passwordHash;
        Role = role;
        IsActive = true;
        TokenVersion = 1;
        IsSystemUser = isSystemUser;
        CreatedAt = DateTime.UtcNow;
        Image = string.IsNullOrWhiteSpace(image) ? null : image.Trim();
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("Password cannot be empty.");

        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeEmail(string newEmail)
    {
        ValidateEmail(newEmail);
        Email = newEmail.ToLowerInvariant().Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(bool isActive)
    {
        if (IsSystemUser)
            throw new DomainException("System user status cannot be changed.");

        if (Role != UserRole.ProductManager)
            throw new DomainException("Only ProductManager status can be changed.");

        if (IsActive == isActive)
            return;

        IsActive = isActive;

        if (!isActive)
            InvalidateAllSessions();

        UpdatedAt = DateTime.UtcNow;
    }

    public void InvalidateAllSessions()
    {
        TokenVersion++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsSystemUser()
    {
        if (IsSystemUser)
            return;

        IsSystemUser = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeImage(string? image)
    {
        Image = string.IsNullOrWhiteSpace(image) ? null : image.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    private void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty.");

        // یک رگکس ساده برای اطمینان از فرمت صحیح ایمیل
        var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(email, emailRegex))
            throw new DomainException("Invalid email format.");
    }
}
