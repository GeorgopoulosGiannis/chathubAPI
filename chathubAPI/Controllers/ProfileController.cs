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
        readonly IRelationshipRepo _relationshipRepo;

        public ProfileController(IProfileRepo profileRepo, IUserRepo userRepo, IRelationshipRepo relationshipRepo)
        {
            _profileRepo = profileRepo;
            _userRepo = userRepo;
            _relationshipRepo = relationshipRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string email)
        {
            if (email != null)
            {
                string userId = _userRepo.GetUserIdFromEmail(email);
                if (userId != null)
                {

                    Profile profile = _profileRepo.Get(userId);
                    ProfileDTO profDTO = new ProfileDTO
                    {
                        Alias = profile.Alias,
                        Avatar = profile.Avatar,
                        Description = profile.Description,
                        Email = email
                    };
                    return Ok(profDTO);
                }
            }
            return NotFound();
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(ProfileDTO prof)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Profile oldProf = _profileRepo.Get(userId);
            oldProf.Alias = prof.Alias;
            oldProf.Avatar = prof.Avatar;
            oldProf.Description = prof.Description;
            if (_profileRepo.Update(oldProf))
            {
                _profileRepo.Save();
                return Ok();

            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSuggestions(int currentPage = 1)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Profile> profs = _profileRepo.GetRandomProfiles(userId, currentPage);
            List<Profile> cloneProfs = profs.ToList();
            List<Relationship> rels = _relationshipRepo.GetRelationshipsAllStatus(userId);
            List<ProfileDTO> profsDTO = new List<ProfileDTO>();

        

                foreach (var prof in profs)
                {
                    foreach (var rel in rels)
                    {
                        if (prof.UserId == rel.User_OneId || prof.UserId == rel.User_TwoId)
                        {
                            cloneProfs.Remove(prof);
                        }
                    }

                }
            
            if (cloneProfs.Count < 20)
            {
                currentPage += 1;
                List<Profile> newProfs = _profileRepo.GetRandomProfiles(userId, currentPage);
                while (profs.Count < 20 && newProfs.Count == 20)
                {
                    foreach (var prof in newProfs)
                    {
                        profs.Add(prof);
                    }
                }

            }
            foreach (var prof in cloneProfs)
            {
                profsDTO.Add(new ProfileDTO
                {
                    Email = _userRepo.GetUserEmailFromId(prof.UserId),
                    Alias = prof.Alias,
                    Description = prof.Description,
                    Avatar = prof.Avatar
                });

            }

            return Ok(profsDTO);
        }
    }
}