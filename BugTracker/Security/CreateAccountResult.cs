namespace BugTracker.Security
{
    public class CreateAccountResult
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}