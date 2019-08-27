using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using chathubAPI.DTO;
using chathubAPI.Helpers;
using chathubAPI.Hubs;
using chathubAPI.Models;
using chathubAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace chathubAPI.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {

        readonly UserManager<IdentityUser> _userManager;
        readonly IRelationshipRepo _relationshipRepo;
        readonly IUserRepo _userRepo;
        readonly IHubContext<ChatHub> _hubContext;
        readonly IProfileRepo _profileRepo;

        public FriendsController(UserManager<IdentityUser> userManager, IRelationshipRepo relationshipRepo, IUserRepo userRepo, IHubContext<ChatHub> hubContext, IProfileRepo profileRepo)
        {
            _userManager = userManager;
            _relationshipRepo = relationshipRepo;
            _userRepo = userRepo;
            _hubContext = hubContext;
            _profileRepo = profileRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int status)
        {
            if (status >= 0 && status <= 3)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                List<Relationship> rels = _relationshipRepo.GetRelationships(userId, status);
                List<ProfileDTO> profsDTO = new List<ProfileDTO>();

                foreach (var rel in rels)
                {
                    Profile prof = new Profile();
                    ProfileDTO profDTO = new ProfileDTO();
                    if (userId == rel.User_OneId)
                    {
                        prof = _profileRepo.Get(rel.User_TwoId);
                        profDTO.Alias = prof.Alias;
                        profDTO.Avatar = prof.Avatar;
                        profDTO.Email = _userRepo.GetUserEmailFromId(rel.User_TwoId);
                        profDTO.Description = prof.Description;

                    }
                    else
                    {
                        prof = _profileRepo.Get(rel.User_OneId);
                        profDTO.Alias = prof.Alias;
                        profDTO.Avatar = prof.Avatar;
                        profDTO.Email = _userRepo.GetUserEmailFromId(rel.User_OneId);
                        profDTO.Description = prof.Description;
                    }
                    profsDTO.Add(profDTO);
                }

                return Ok(profsDTO);
            }
            else
            {
                return BadRequest();
            }


        }


        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]RelationshipDTO invitation)
        {
            if (invitation.Email != null && (invitation.Status == 0 || invitation.Status == 3))
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                Relationship relationship = new Relationship
                {
                    User_OneId = userId,
                    User_TwoId = _userRepo.GetUserIdFromEmail(invitation.Email),
                    Action_UserId = userId,
                    Status = invitation.Status
                };
                if (_relationshipRepo.Add(relationship))
                {

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateStatus([FromBody]RelationshipDTO updateStatus)
        {
            if (updateStatus.Email != null && (updateStatus.Status == 1 || updateStatus.Status == 2))
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Relationship relationship = new Relationship
                {
                    User_OneId = userId,
                    User_TwoId = _userRepo.GetUserIdFromEmail(updateStatus.Email),
                    Action_UserId = userId,
                    Status = updateStatus.Status
                };
                if (_relationshipRepo.UpdateStatus(relationship))

                    return Ok(updateStatus.Status);

                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

    }
}