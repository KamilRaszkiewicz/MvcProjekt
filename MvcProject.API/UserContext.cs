namespace MvcProject.API
{
    public class UserContext
    {
        public int? Id { get; init; } = null;
        public string? Email { get; init; } = null;
        public bool? IsVerified { get; init; } = null;
    }
}
