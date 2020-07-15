using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using course_app.Data;
using course_app.Dtos;
using course_app.Helpers;
using course_app.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace course_app.Controllers
{
    [Route("api/users/{userId}/photo")]
    [ApiController]
    public class PhotoController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IDatingRepository _repo;
        private readonly IOptions<CloudinarySettings> _cloudinarySettings;
        private readonly Cloudinary _cloudinary;

        public PhotoController(IMapper mapper, IOptions<CloudinarySettings> cloudinarySettings, IDatingRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
            _cloudinarySettings = cloudinarySettings;

            var account = new Account
            {
                ApiKey = cloudinarySettings.Value.ApiKey,
                ApiSecret = cloudinarySettings.Value.ApiSecret,
                Cloud = cloudinarySettings.Value.CloudName
            };

            _cloudinary = new Cloudinary(account);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoToUser(int userId, [FromForm] PhotoForCreationDto uploadDto) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

            var userFromRepoTask = _repo.GetUserWithPhotos(userId);

            var file = uploadDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                };

                uploadResult = _cloudinary.Upload(uploadParams);
            }

            uploadDto.Url = uploadResult.Url.ToString();
            uploadDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(uploadDto);

            var userFromRepo = await userFromRepoTask;

            userFromRepo.Photos.Add(photo);

            if (!userFromRepo.Photos.Any(p => p.IsMain)) photo.IsMain = true;
            
            if (await _repo.SaveAll())
            {
                var photoDto = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { userId = userId, id = photo.Id }, photoDto);
            }

            throw new Exception($"Could not add photo to user with id {userId}");
        }
    }
}
