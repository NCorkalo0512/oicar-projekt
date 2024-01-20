using FriendFinderAPI.Model;
using FriendFinderAPI.Model.DTO;

namespace FriendFinderAPI.Interfaces
{
    public interface IUserRepository
    {
        User Authenticate(string email, string password);
        IEnumerable<UserDTO> GetUsers();
        User GetUser(string username);
        UserDTO GetById(int id);
        User Create(User user, string password);
        void Update(User user, string password = null, string passwordOld = null);
        void Delete(int id);
        string GetUserType(int id);
      
    }
}
