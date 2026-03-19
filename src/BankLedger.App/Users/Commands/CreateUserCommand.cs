public sealed class CreateUserCommand
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required DateOnly BirthDate { get; init; }
    public String? PhoneNumber { get; init; }
}