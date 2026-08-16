using System.Text.RegularExpressions;
using Task_Domain.Common;

namespace Task_Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    // سازنده بدون پارامتر برای EF
    private User() { }

    // سازنده اصلی
    public User(string username, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username cannot be empty.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password cannot be empty.");

        // ولیدیشن اولیه ایمیل در لایه دمین
        ValidateEmail(email);

        Id = Guid.NewGuid();
        Username = username;
        Email = email.ToLower().Trim();
        PasswordHash = passwordHash;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("Password cannot be empty.");

        PasswordHash = newPasswordHash;
    }

    public void ChangeEmail(string newEmail)
    {
        ValidateEmail(newEmail);
        Email = newEmail.ToLower().Trim();
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
