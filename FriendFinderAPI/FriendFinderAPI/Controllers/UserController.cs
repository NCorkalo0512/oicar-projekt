using FriendFinderAPI.Interfaces;
using FriendFinderAPI.Model;
using FriendFinderAPI.Model.Command;
using FriendFinderAPI.Model.DTO;
using FriendFinderAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FriendFinderAPI.Controllers
{
    /// <summary>
    /// Defines the <see cref="UserController" />.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        public IUnitOfWork _unitOfWorK { get; set; }

        private readonly JWTSettings _jwtSettings;

        public UserController(IUnitOfWork unitOfWork, IOptions<JWTSettings> jwtsettings)
        {

            _unitOfWorK = unitOfWork;
            _jwtSettings = jwtsettings.Value;
        }

        /// <summary>
        /// Authenticate user by sending email and password in request body. If authorized this will return bearer header token property for further authorization
        /// </summary>
        /// <param name="model">The model<see cref="AuthenticateUserCommand"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateUserCommand model)
        {
            var user = _unitOfWorK.UserRepository.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            if (user.Verification == false)
                return BadRequest(new { message = "Your account hasn't been verified" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Ovo je tajni kljuc projekta");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Iduser.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
           var userDTO= new UserDTO
            {
                Iduser = user.Iduser,
                IdUserProfile = user.Profiles?.FirstOrDefault()?.Idprofile ??0 ,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreateDateTime = user.CreateDateTime,
                UserType = user.UserTypeId.HasValue ? _unitOfWorK.UserRepository.GetUserType((int)user.UserTypeId):string.Empty,
                Token = tokenString,
                Technology = user.Profiles?.FirstOrDefault()?.Technology,
                ProjectDescription = user.Profiles?.FirstOrDefault()?.ProjectDescription
            };

            return Ok(userDTO);
        }
        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="model">The model<see cref="RegisterUserCommand"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserCommand model)
        {
            // map model to entity
            var user = new User()
            {
                CreateDateTime = DateTime.UtcNow,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserTypeId = 2
            };
            user.Verification = true;

          

            try
            {
                // create user
                _unitOfWorK.UserRepository.Create(user, model.Password);
                var profile = new CreateProfileCommand()
                {
                    Technology = model.Technology,
                    ProjectDescription = model.ProjectDescription
                };
                var userId = _unitOfWorK.UserRepository.GetUser(model.Email).Iduser;
                profile.UserId = userId;
                _unitOfWorK.ProfileRepository.CreateProfile(profile);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
            
        }
        // GET: api/Users/5
        /// <summary>
        /// Gets one user
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{ActionResult{User}}"/>.</returns>
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _unitOfWorK.UserRepository.GetById(id);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
            /* try
             {
                 // get user
                 var user = _unitOfWorK.UserRepository.GetById(id);
                 if (user == null)
                     return NotFound();
                 return Ok(user);
             }
             catch (ApplicationException ex)
             {
                 // return error message if there was an exception
                 return NotFound(new { message = ex.Message });
             }*/
        }

        /// <summary>
        /// Gets one user
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{ActionResult{User}}"/>.</returns>
        [HttpGet("all")]
        public IActionResult GetUsers()
        {
            try
            {
                // get user
                var user = _unitOfWorK.UserRepository.GetUsers();
                return Ok(user);
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update password
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <param name="model">The model<see cref="UpdateUserPasswordCommand"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpPut("{id}/password")]
        public IActionResult UpdatePassword(int id, [FromBody] UpdateUserPasswordCommand model)
        {
            // map model to entity and set id
            var user = new User();
            user.Iduser = id;

            try
            {
                if (!string.IsNullOrWhiteSpace(model.Password) && !string.IsNullOrEmpty(model.OldPassword))
                {
                    // update user 
                    _unitOfWorK.UserRepository.Update(user, model.Password, model.OldPassword);
                    return Ok();
                }
                else
                {
                    return BadRequest("There was a problem with passwords(Check if null or property name correctly spelled)");
                }
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
       
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _unitOfWorK.UserRepository.Delete(id);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            /* var user = _unitOfWorK.UserRepository.GetById(id);
             if (user != null)
             {
                 return NotFound();
             }

             user.FirstName = "NoUser";
             user.LastName = "Deleted";
             user.Email = "no email";

             _unitOfWorK.ProfileRepository.ToggleProfile(user.Iduser);

             return Ok();*/
        }

      







    }
}
