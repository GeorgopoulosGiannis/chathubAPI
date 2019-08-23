using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using chathubAPI.DTO;
using chathubAPI.Models;
using chathubAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public FriendsController(UserManager<IdentityUser> userManager, IRelationshipRepo relationshipRepo, IUserRepo userRepo)
        {
            _userManager = userManager;
            _relationshipRepo = relationshipRepo;
            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int status)
        {
            if (status >= 0 && status <= 3)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                List<Relationship> rels = _relationshipRepo.GetRelationships(userId, status);
                List<RelationshipDTO> relsDTO = new List<RelationshipDTO>();
               
                foreach (var rel in rels)
                {
                    RelationshipDTO relDTO = new RelationshipDTO();
                    if (userId == rel.User_OneId)
                    {
                        relDTO.Email = GetUserEmailFromId(rel.User_TwoId);
                    }
                    else
                    {
                        relDTO.Email = GetUserEmailFromId(rel.User_OneId);
                    }
                    relDTO.Status = rel.Status;
                    relsDTO.Add(relDTO);
                }

                return Ok(relsDTO);

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
                    User_TwoId = GetUserIdFromEmail(invitation.Email),
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
        public async Task<IActionResult> UpdateStatus([FromBody]string userTo, int status)
        {
            if (userTo != null && (status == 1 || status == 2))
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Relationship relationship = new Relationship
                {
                    User_OneId = userId,
                    User_TwoId = GetUserIdFromEmail(userTo),
                    Action_UserId = userId,
                    Status = status
                };
                if (_relationshipRepo.UpdateStatus(relationship))
                {
                    return Ok(status);
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

        private string GetUserIdFromEmail(string email)
        {
            return _userRepo.GetUserIdFromEmail(email);
        }
        private string GetUserEmailFromId(string userId)
        {
            return _userRepo.GetUserEmailFromId(userId);
        }

    }
}