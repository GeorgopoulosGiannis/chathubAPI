using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using chathubAPI.DATA;
using chathubAPI.INTERFACES;
using chathubAPI.Models;
using chathubAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace chathubAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly UserManager<IdentityUser> _userManager;
        readonly SignInManager<IdentityUser> _signInManager;
        readonly IProfileRepo _profileRepo;
        readonly IFcmTokenRepo _fcmTokenRepo;

        public UserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IFcmTokenRepo fcmTokenRepo,
            IProfileRepo profileRepo
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _profileRepo = profileRepo;
            _fcmTokenRepo = fcmTokenRepo;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Credentials credentials)
        {

            try
            {
                var user = new User { UserName = credentials.Email, Email = credentials.Email };

                var result = await _userManager.CreateAsync(user, credentials.Password);

                if (!result.Succeeded)
                {

                    return Conflict(result.Errors);
                }

                _profileRepo.Add(user);
                string token = CreateToken(user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Credentials credentials)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(credentials.Email, credentials.Password, false, false);
                if (!result.Succeeded)
                {
                    return Unauthorized();
                }
                else
                {
                    var user = await _userManager.FindByEmailAsync(credentials.Email);
                    return Ok(CreateToken(user));
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }
        [Authorize]
        [HttpPost("fcmToken")]
        public async Task<IActionResult> AddToken([FromBody]FcmToken fcmToken)
        {
        
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                fcmToken.TokenOwner = userId;
                FcmToken existingToken = await _fcmTokenRepo.Read(fcmToken.Token);
                if (existingToken != null && existingToken.TokenOwner != fcmToken.TokenOwner)
                {
                    await _fcmTokenRepo.Update(fcmToken.Token, fcmToken.TokenOwner);
                    return Ok();

                }
                else if (existingToken == null)
                {
                    await _fcmTokenRepo.Create(fcmToken.Token, fcmToken.TokenOwner);
                    return Ok();
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        string CreateToken(IdentityUser user)
        {
            var claims = new Claim[]
          {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
          };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is the secret phrase"));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(signingCredentials: signingCredentials, claims: claims, expires: DateTime.UtcNow.AddMinutes(60));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}