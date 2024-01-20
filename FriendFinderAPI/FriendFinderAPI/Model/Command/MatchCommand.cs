namespace FriendFinderAPI.Model.Command
{
    public class MatchCommand
    {
        public string SwiperUser { get; set; }
        public string SwippedUser { get; set; }

        public bool Swipe { get; set; }
    }
}
