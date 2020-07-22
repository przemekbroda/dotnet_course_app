using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using course_app.Data;
using course_app.Dtos;
using course_app.Models;
using course_app.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace course_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IJwtTokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repo, IJwtTokenService tokenService, IConfiguration config, IMapper mapper)
        {
            _repo = repo;
            _tokenService = tokenService;
            _config = config;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto registerDto) 
        {
            //TODO validate request

            registerDto.Username = registerDto.Username.ToLower();

            if (await _repo.UserExists(registerDto.Username)) return BadRequest("User already exists");

            var userToCreate = _mapper.Map<User>(registerDto);

            var createdUser = await _repo.Register(userToCreate, registerDto.Password);

            var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser);

            return CreatedAtRoute("GetUser", new { controller = "Users", id = createdUser.Id }, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto loginDto) 
        {
            var userFromRepo = await _repo.Login(loginDto.Username, loginDto.Password);

            if (userFromRepo == null) return Unauthorized();

            return Ok(new
            {
                token = _tokenService.GenerateAccessToken(userFromRepo, DateTime.UtcNow.AddDays(1)),
                refreshToken = _tokenService.GenerateRefreshToken(userFromRepo, DateTime.UtcNow.AddDays(30))
            }); 
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto) 
        {
            int id;

            if (!_tokenService.ValidateRefreshToken(refreshTokenDto.RefreshToken, out id)) Unauthorized();

            var userFromRepo = await _repo.FindById(id);

            if (userFromRepo == null) return Unauthorized();

            return Ok(new {
                token = _tokenService.GenerateAccessToken(userFromRepo, DateTime.Now.AddDays(1)),
                refreshToken = _tokenService.GenerateRefreshToken(userFromRepo, DateTime.Now.AddDays(30))
            });
        }
    }
}
