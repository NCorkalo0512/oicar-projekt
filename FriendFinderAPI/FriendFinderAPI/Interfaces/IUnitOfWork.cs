namespace FriendFinderAPI.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IProfileRepository ProfileRepository { get; }
        public IMatchRepository MatchRepository { get; }

        public IMessageRepository MessageRepository { get; }

    }
}
