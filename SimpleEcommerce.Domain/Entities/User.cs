using SimpleEcommerce.Domain.Common;
using SimpleEcommerce.Domain.Enums;
using SimpleEcommerce.Domain.ValueObjects;

namespace SimpleEcommerce.Domain.Entities;

public class User : BaseEntity
{
    private User() { }

    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public UserRole Role { get; private set; }

    public User(Email email, Password password, string firstName, string lastName, UserRole role = UserRole.User)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;

        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
    }

    public void ChangePassword(Password newPassword)
    {
        Password = newPassword;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
    }
}