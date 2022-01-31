namespace BugTracker.Security
{
    public class LoginResult
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}