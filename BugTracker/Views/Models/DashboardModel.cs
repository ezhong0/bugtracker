namespace BugTracker.Views.Models
{
    public class DashboardModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int ProjectId { get; set; }
        //public string Title { get; set; }
        //public string Description { get; set; }
        public string DateModified { get; set; }
        public string JoinCode { get; set; }
        public int UserId { get; set; }
    }
}