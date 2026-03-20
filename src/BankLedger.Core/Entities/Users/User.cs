public sealed record User
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Email { get; init; } = "";
    public string EmailNormalized { get; init; } = "";
    public string PasswordHash { get; init; } = "";
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? LastLogin { get; init; }
    public UserStatus Status { get; init; } = UserStatus.Active;
    public bool IsEmailVerified { get; init; } = false;
    public string? PhoneNumber { get; init; }
    public bool IsPhoneNumberVerified { get; init; } = false;
    public DateOnly BirthDate { get; init; }

    public User(string name, string email, string passwordHash, DateOnly birthDate, string? phoneNumber = "")
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        EmailNormalized = email.ToUpperInvariant();
        PasswordHash = passwordHash;
        BirthDate = birthDate;
        phoneNumber = phoneNumber;
    }


}