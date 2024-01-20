namespace FriendFinderAPI.Model.DTO
{
    public class MatchDTO
    {
        public int Idmatching { get; set; }

        public bool? Swipe { get; set; }

        public int? UserId { get; set; }

        public string? Technology { get; set; }

        public string? ProjectDescription { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateTime { get; set; }

       
    }
}
