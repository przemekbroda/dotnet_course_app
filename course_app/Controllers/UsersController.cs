using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using course_app.Data;
using course_app.Dtos;
using course_app.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace course_app.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _datingRepo;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository datingRepository, IMapper mapper)
        {
            _datingRepo = datingRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _datingRepo.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }    

            var users = await _datingRepo.GetUsers(userParams.PageNumber, userParams.PageSize);

            var usersDto = _mapper.Map<PagedListDto<UserForDetailedDto>>(users);

            return Ok(usersDto);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _datingRepo.GetUserWithPhotos(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto updateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

            var usetFromRepo = await _datingRepo.GetUserWithPhotos(id);

            _mapper.Map(updateDto, usetFromRepo);

            if (await _datingRepo.SaveAll()) 
            {
                return Ok();
            }

            throw new Exception($"Could not update user with id {id}");
        }

        [HttpGet("example/{id}")]
        public async Task<IActionResult> Example(int id)
        {
            var user = await _datingRepo.Test(id);

            if (user != null)
            {
                var userDto = _mapper.Map<UserForDetailedDto>(user);
                return Ok(userDto);
            }

            throw new Exception($"User with id {id} was not found");
        }

    }
}
