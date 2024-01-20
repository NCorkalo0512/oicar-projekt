using FriendFinderAPI.Interfaces;
using FriendFinderAPI.Model.Command;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendFinderAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        public IUnitOfWork _unitOfWorK { get; set; }

        public MessageController(IUnitOfWork unitOfWork)
        {
            _unitOfWorK = unitOfWork;
        }

        /// <summary>
        /// Get messages for matched user
        /// </summary>
        /// <param name="senderId">The model<see cref="int"/>.</param>
        /// <param name="receiverId">The model<see cref="int"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpGet("sender/{senderId}/receiver/{receiverId}")]//api/message
        public IActionResult GetMessagesForMatchedUser(int senderId, int receiverId)
        {
            try
            {
                // get messages
                var messages = _unitOfWorK.MessageRepository.GetAllMessagesForMatchedUser(senderId,receiverId);
                return Ok(messages);
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Send messages for matched user
        /// </summary>
        /// <param name="senderId">The model<see cref="int"/>.</param>
        /// <param name="receiverId">The model<see cref="int"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpPost("send")]//api/message
        public IActionResult SendMessageForMatchedUser([FromBody]NewMessageCommand command)
        {
            try
            {
                // get messages
                _unitOfWorK.MessageRepository.SendMessageForMatchedUser(command.SenderId, command.ReceiverId,command.Text);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete message
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
           

            _unitOfWorK.MessageRepository.DeleteMessageForMatchedUser(id);

            return Ok();
        }


        /// <summary>
        /// Report messages for matched user
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpPost("report")]//api/message
        public IActionResult ReportMessageForMatchedUser([FromBody] MessageReportCommand messageReport)
        {
            try
            {
                // report messages
                _unitOfWorK.MessageRepository.ReportMessage(messageReport);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get report messages
        /// </summary>    
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpGet("reported/all")]//api/message
        public IActionResult GetAllReportedMessages()
        {
            try
            {
                // get reported messages
                var messages = _unitOfWorK.MessageRepository.GetAllReportedMessages();
                return Ok(messages);
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
