using FriendFinderAPI.Controllers;
using FriendFinderAPI.Interfaces;
using FriendFinderAPI.Model;
using FriendFinderAPI.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FriendFinderAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        public FriendFinderDbContext _context { get; set; }
        public UserRepository(FriendFinderDbContext context)
        {
            _context = context;
        }

        public User GetUser(string username)
        {
           return _context.Users.Include(u => u.Profiles).FirstOrDefault(u => u.Email==username);
            
        
        }

        public User Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.Include(u => u.Profiles).FirstOrDefault(x => x.Email == email);

            /*provjeriti postoji li user*/
            if (user == null)
                return null;

            /*provjeriti je li tocna lozinka*/
            byte[] data = user.PasswordHash;
            int i = data.Length - 1;
            while (i >= 0 && data[i] == 0x00)
            {
                i--;
            }
            Array.Resize(ref data, i + 1);
            if (!VerifyPasswordHash(password, data))
                return null;

            /*uspjesno*/
            return user;

        }

        private bool VerifyPasswordHash(string password, byte[]? storedHash)
        {

            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            var hash = Encoding.UTF8.GetString(storedHash, 0, storedHash.Length);
            return PasswordHasher.Verify(password, hash);

        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var users = _context.Users.Include(u => u.Profiles).ToList();
            var usersDTO = new List<UserDTO>();
            foreach(var user in users)
            {
                var profile= user.Profiles.FirstOrDefault();
                usersDTO.Add(new UserDTO()
                {
                    Iduser = user.Iduser,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreateDateTime = user.CreateDateTime,
                    UserType = GetUserType((int)user.UserTypeId),
                    Technology=profile.Technology,
                    ProjectDescription=profile.ProjectDescription,
                    Enabled=profile.Enabled
                    
                    
                });
            }
            return usersDTO;
        }

        public UserDTO GetById(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
                throw new ApplicationException("User not found");
            var newUser = new UserDTO()
            {
                Iduser = user.Iduser,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreateDateTime = user.CreateDateTime,
                UserType = GetUserType((int)user.UserTypeId)
            };
            return newUser;
        }

        public User Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ApplicationException("Password is required");


            if (_context.Users.Any(x => x.Email == user.Email))
                throw new ApplicationException("Email \"" + user.Email + "\" is already taken");

            byte[] passwordHash = System.Text.Encoding.UTF8.GetBytes(PasswordHasher.Hash(password));

            user.PasswordHash = passwordHash;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;

        }

        public void Update(User userParam, string password = null, string passwordOld = null)
        {
            var user = _context.Users.Find(userParam.Iduser);

            if (user == null)
                throw new ApplicationException("User not found");


            if (!string.IsNullOrWhiteSpace(userParam.Email) && userParam.Email != user.Email)
            {

                if (_context.Users.Any(x => x.Email == userParam.Email))
                    throw new ApplicationException("Email " + userParam.Email + " is already taken");

                user.Email = userParam.Email;
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.FirstName))
                user.FirstName = userParam.FirstName;

            if (!string.IsNullOrWhiteSpace(userParam.LastName))
                user.LastName = userParam.LastName;



            if (!string.IsNullOrWhiteSpace(password) && !string.IsNullOrEmpty(passwordOld))
            {
                if (PasswordHasher.Verify(password, System.Text.Encoding.UTF8.GetString(user.PasswordHash)))
                {
                    byte[] passwordHash = System.Text.Encoding.UTF8.GetBytes(PasswordHasher.Hash(password));
                    user.PasswordHash = passwordHash;
                }
                else
                {
                    throw new ApplicationException("Incorrect old password");
                }
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Include(x => x.Profiles).FirstOrDefault(x => x.Iduser == id);
            if (user != null)
            {
                user.FirstName = "Deleted";
                user.LastName = "User";
                user.Email = "DELETED EMAIL";
                user.Profiles.FirstOrDefault().Technology = "Deleted Technology";
                user.Profiles.FirstOrDefault().ProjectDescription = "Deleted Description";
            }
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public string GetUserType(int id)
        {
            return _context.UserTypes.Find(id).Value;
        }

      
    }
}
