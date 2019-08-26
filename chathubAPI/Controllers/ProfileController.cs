using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using chathubAPI.DTO;
using chathubAPI.Models;
using chathubAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chathubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProfileController : ControllerBase
    {
        readonly IProfileRepo _profileRepo;
        readonly IUserRepo _userRepo;

        public ProfileController(IProfileRepo profileRepo, IUserRepo userRepo)
        {
            _profileRepo = profileRepo;
            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string email)
        {
            if (email != null)
            {

                return Ok(_profileRepo.Get(_userRepo.GetUserIdFromEmail(email)));

            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProfileDTO prof)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Profile oldProf = _profileRepo.Get(userId);
            oldProf.Alias = prof.Alias;
            oldProf.Avatar = prof.Avatar;
            oldProf.Description = prof.Description;
            if (_profileRepo.Update(oldProf))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
            
        }
    }
}