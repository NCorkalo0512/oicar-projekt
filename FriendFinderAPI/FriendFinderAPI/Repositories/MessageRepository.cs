using FriendFinderAPI.Interfaces;
using FriendFinderAPI.Model;
using FriendFinderAPI.Model.Command;
using FriendFinderAPI.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace FriendFinderAPI.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        public FriendFinderDbContext _context { get; set; }
        public MessageRepository(FriendFinderDbContext context)
        {
            _context = context;
        }

        public List<MessageDTO> GetAllMessagesForMatchedUser(int senderID, int reciverID)
        {
            var messagesSent = _context.Messages.Include(x => x.DeletedMessages).Include(x => x.UserReceiver)
                .Include(x => x.UserSender).Where(x => (x.UserSenderId == senderID && x.UserReceiverId == reciverID)).ToList();

            var messagesReceived = _context.Messages.Include(x => x.DeletedMessages).Include(x => x.UserReceiver)
                .Include(x => x.UserSender).Where(x => (x.UserSenderId == reciverID && x.UserReceiverId == senderID)).ToList();

            var allMessagesList = new List<MessageDTO>();
            if (messagesSent != null)
            {
                foreach (var message in messagesSent)
                {
                    var newMessage = new MessageDTO()
                    {
                        DateTime = message.DateTime,
                        Idmessage = message.Idmessage,
                        Text = message.DeletedMessages != null && message.DeletedMessages.Any() ? "Deleted message" : message.Text,
                        IsSender = message.UserSenderId == senderID,
                        UserReceiverId = message.UserReceiverId,
                        UserSenderId = message.UserSenderId,
                        IsDeleted = message.DeletedMessages != null && message.DeletedMessages.Any() ? true : false,
                        UserReceiverFullName = message.UserReceiver?.FirstName + ' ' + message.UserReceiver?.LastName,
                        UserSenderFullName = message.UserSender?.FirstName + ' ' + message.UserSender?.LastName
                    };
                    allMessagesList.Add(newMessage);
                }
            }

            if (messagesReceived != null)
            {
                foreach (var message in messagesReceived)
                {
                    var newMessage = new MessageDTO()
                    {
                        DateTime = message.DateTime,
                        Idmessage = message.Idmessage,
                        Text = message.DeletedMessages != null && message.DeletedMessages.Any() ? "Deleted message" : message.Text,
                        IsSender = message.UserSenderId == senderID,
                        UserReceiverId = message.UserReceiverId,
                        UserSenderId = message.UserSenderId,
                        IsDeleted = message.DeletedMessages != null && message.DeletedMessages.Any() ? true : false,
                        UserReceiverFullName = message.UserReceiver?.FirstName + ' ' + message.UserReceiver?.LastName,
                        UserSenderFullName = message.UserSender?.FirstName + ' ' + message.UserSender?.LastName
                    };
                    allMessagesList.Add(newMessage);
                }
            }

            allMessagesList = allMessagesList.OrderBy(x => x.DateTime).ToList();
            return allMessagesList;
        }

        public void SendMessageForMatchedUser(int senderID, int reciverID, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var newMessage = new Message()
                {
                    DateTime = DateTime.UtcNow,
                    Text = text,
                    UserSenderId = senderID,
                    UserReceiverId = reciverID
                };

                _context.Messages.Add(newMessage);
                _context.SaveChanges();
            }

        }

        public void DeleteMessageForMatchedUser(int messageID)
        {
            var message = _context.Messages.FirstOrDefault(x => x.Idmessage == messageID);
            var messageDeleted = _context.DeletedMessages.FirstOrDefault(x => x.MessageId == messageID);
            if (message != null && messageDeleted==null)
            {
                var messageDelete = new DeletedMessage()
                {
                    DateTime = DateTime.UtcNow,
                    MessageId = messageID
                };

                _context.DeletedMessages.Add(messageDelete);
                _context.SaveChanges();
            }
        }

        public void ReportMessage(MessageReportCommand messageReport)
        {
            var message = _context.Messages.FirstOrDefault(x => x.Idmessage == messageReport.MessageId);
            if (message != null)
            {
                var newMessageReport = new MessageReport()
                {
                    DateTime = DateTime.UtcNow,
                    MessageId = messageReport.MessageId,
                    Reason = messageReport.Reason,
                    UserSenderId = messageReport.UserSenderId
                };

                _context.MessageReports.Add(newMessageReport);
                _context.SaveChanges();
            }
        }

        public List<MessageReportDTO> GetAllReportedMessages()
        {
            var reportedMessages = _context.MessageReports.Include(x => x.UserSender).ToList();

            var allReportedMessages = new List<MessageReportDTO>();
            if (reportedMessages != null && reportedMessages.Any())
            {
                foreach (var reportedMessage in reportedMessages)
                {
                    var message = new MessageReportDTO()
                    {
                        IdmessageReport = reportedMessage.IdmessageReport,
                        MessageId = reportedMessage.MessageId,
                        Reason = reportedMessage.Reason,
                        DateTime = reportedMessage.DateTime,
                        UserSenderId = reportedMessage.UserSenderId,
                        UserSenderFullName = reportedMessage.UserSender?.FirstName + ' ' + reportedMessage.UserSender?.LastName

                    };
                    allReportedMessages.Add(message);
                }
            }
            allReportedMessages = allReportedMessages.OrderBy(x => x.DateTime).ToList();
            return allReportedMessages;
        }
    }
}
