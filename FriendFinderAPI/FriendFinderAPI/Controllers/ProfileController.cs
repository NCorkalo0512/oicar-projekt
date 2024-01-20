using FriendFinderAPI.Interfaces;
using FriendFinderAPI.Model;
using FriendFinderAPI.Model.Command;
using FriendFinderAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FriendFinderAPI.Controllers
{
    /// <summary>
    /// Defines the <see cref="ProfileController" />.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : Controller 
    {
        public IUnitOfWork _unitOfWorK { get; set; }

        public ProfileController(IUnitOfWork unitOfWork)
        {
            _unitOfWorK = unitOfWork;          
        }

        /// <summary>
        /// Register profile
        /// </summary>
        /// <param name="model">The model<see cref="CreateProfileCommand"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpPost]
        public IActionResult Create([FromBody] CreateProfileCommand model)
        {
          
            try
            {
                // create profile
                var profile = _unitOfWorK.ProfileRepository.CreateProfile(model);
                return Ok(profile);
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        /*Toggle za deleteUser*/

        /// <summary>
        /// Register profile
        /// </summary>
        /// <param name="model">The model<see cref="UpdateProfileCommand"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpPut]
        public IActionResult Update([FromBody] UpdateProfileCommand model)
        {
           try
            {
                // update profile
                _unitOfWorK.ProfileRepository.UpdateProfile(model);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get profile
        /// </summary>
        /// <param name="id">The model<see cref="int"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpGet("{id}")]//api/profile/5
        public IActionResult Get(int id)
        {
            try
            {
                // get profile
                var profile= _unitOfWorK.ProfileRepository.GetProfile(id);
                return Ok(profile);
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// Get profile
        /// </summary>
        
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpGet("all")]//api/profile
        public IActionResult GetProfiles()
        {
            try
            {
                // get profile
                var profiles= _unitOfWorK.ProfileRepository.GetAllProfiles();
                return Ok(profiles);
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
