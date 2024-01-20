using FriendFinderAPI.Model.Command;
using FriendFinderAPI.Model.DTO;

namespace FriendFinderAPI.Interfaces
{
    public interface IMessageRepository
    {
        List<MessageDTO> GetAllMessagesForMatchedUser(int senderID, int reciverID);

        void SendMessageForMatchedUser(int senderID, int reciverID, string text);

        void DeleteMessageForMatchedUser(int messageID);

        void ReportMessage(MessageReportCommand messageReport);

        List<MessageReportDTO> GetAllReportedMessages();


    }
}
