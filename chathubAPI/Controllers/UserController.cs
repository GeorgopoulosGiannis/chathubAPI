using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using chathubAPI.DATA;
using chathubAPI.Models;
using chathubAPI.Repositories;
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

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext dbContext,IProfileRepo profileRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _profileRepo = profileRepo;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Credentials credentials)
        {

            try
            {
                var user = new User { UserName = credentials.Email, Email = credentials.Email };
            
                var result = await _userManager.CreateAsync(user, credentials.Password);
                _profileRepo.Add(user);
                if (!result.Succeeded)
                    return Conflict(result.Errors);


                return Ok(CreateToken(user));
            }
            catch(Exception ex)
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