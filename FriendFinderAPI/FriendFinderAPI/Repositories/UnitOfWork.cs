using FriendFinderAPI.Interfaces;
using FriendFinderAPI.Model;

namespace FriendFinderAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public FriendFinderDbContext _context { get; set; }
        private IUserRepository _userRepository;
        private IProfileRepository _profileRepository;
        private IMatchRepository _matchRepository;
        private IMessageRepository _messageRepository;

        public UnitOfWork()
        {
            _context = new FriendFinderDbContext();
        }

        public IUserRepository UserRepository
            => _userRepository == null ? new UserRepository(_context) : _userRepository;

        public IProfileRepository ProfileRepository
            => _profileRepository == null ? new ProfileRepository(_context) : _profileRepository;

        public IMatchRepository MatchRepository
            => _matchRepository== null? new MatchRepository(_context): _matchRepository;

        public IMessageRepository MessageRepository
            => _messageRepository==null? new MessageRepository(_context): _messageRepository;

    }
}
