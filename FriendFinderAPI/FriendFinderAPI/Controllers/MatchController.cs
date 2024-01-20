using FriendFinderAPI.Interfaces;
using FriendFinderAPI.Model.Command;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendFinderAPI.Controllers
{
    /// <summary>
    /// Defines the <see cref="MatchController" />.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : Controller
    {
        public IUnitOfWork _unitOfWorK { get; set; }

        public MatchController(IUnitOfWork unitOfWork)
        {
            _unitOfWorK = unitOfWork;
        }

        /// <summary>
        /// Get all matched for user
        /// </summary>

        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpGet("all/matched/{id}")]//api/match
        public async Task<IActionResult> GetAllMatchedForUser(int id)
        {
            try
            {
                // get all matches 
                var matches = await _unitOfWorK.MatchRepository.GetAllMatchedForUser(id);
                return Ok(matches);
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get all not matched for user
        /// </summary>

        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpGet("all/notmatched/{id}")]//api/match
        public async Task<IActionResult> GetAllNotMatchedForUser(int id)
        {
            try
            {
                // get all no matches 
                var matches = await _unitOfWorK.MatchRepository.GetAllNotMatchedForUser(id);
                return Ok(matches);
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// SwipeRight 
        /// </summary>
        /// <param name="model">The model<see cref="MatchCommand"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpPost("swipe")]
        public IActionResult SwipeForUser([FromBody] MatchCommand model)
        {

            try
            {
                
               _unitOfWorK.MatchRepository.SwipeForUser(model);
               return Ok();
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
