using FriendFinderAPI.Model.Command;
using FriendFinderAPI.Model.DTO;

namespace FriendFinderAPI.Interfaces
{
    public interface IMatchRepository
    {
        Task<IEnumerable<MatchDTO>> GetAllMatchedForUser(int userId);
        Task<IEnumerable<MatchDTO>> GetAllNotMatchedForUser(int userId);

        void SwipeForUser(MatchCommand match);
    }
}
