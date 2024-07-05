using BuildingBlocks.Exceptions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.Application.Abstractions;
using DatingApp.Application.Dtos;
using DatingApp.Application.Helpers;
using DatingApp.Application.Interfaces;
using DatingApp.Application.Interfaces.Repositories;
using DatingApp.Domain.Aggregates.AppUser.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DatingApp.Application.Services
{
    public class PhotoAppService : IPhotoAppService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PhotoAppService(IOptions<CloudinarySettings> config , IUserRepository userRepository , IUnitOfWork unitOfWork)
        {
            var acc = new Account(
               config.Value.CloudName,
               config.Value.ApiKey,
               config.Value.ApiSecret
               );
            _cloudinary = new Cloudinary(acc);
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ImageUploadResult> AddPhoto(IFormFile formFile , string username)
        {
            var user =await _userRepository.FindByUserName(username);

            if (user == null) throw new NotFoundException("User Not Found"); 

            var uploadResult = new ImageUploadResult();
            if (formFile.Length > 0)
            {
                using var stream = formFile.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(formFile.FileName,stream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                    .Gravity("face"),
                    Folder = "da-net-8"

                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            var photo = new UserPhoto
            {
                Url = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId,
            };

            if (user.Photos.Count == 0) {
                _ = photo.IsMain == true;
            } 

            user.Photos.Add(photo);
            await _unitOfWork.SaveChangesAsync();
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhoto(string publicId)
        {
            var deleteParam = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParam);
        }

        public async Task DeletePhotoAsync(int photoId , string userName)
        {
            var user = await _userRepository.FindByUserName(userName);
            if (user == null) throw new NotFoundException("User Not Found");

            var photo = user.Photos.FirstOrDefault(p=>p.Id==photoId);
            if (photo is null) throw new NotFoundException("the photo is not exist");

            if (photo.IsMain) throw new Exception("You Can Not Deelete The Main Photo");

            if (photo.PublicId !=null)
            {
                try
                {
                   await DeletePhoto(photo.PublicId);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            user.Photos.Remove(photo);  // ef track this entity(user)
           await  _unitOfWork.SaveChangesAsync();
        }

        public async Task SetUserMainPhoto( int photoId ,string userName)
        {
            var user = await _userRepository.FindByUserName(userName);
            if (user == null) throw new NotFoundException("User Not Found");

          var userPhoto = user.Photos.FirstOrDefault(user => user.Id == photoId);

            if (userPhoto is { } && !userPhoto.IsMain)
            {
                var currentMainPhoto = user.Photos.FirstOrDefault(p=>p.IsMain);

                if (currentMainPhoto is not null)
                    currentMainPhoto.IsMain = false;   

                userPhoto.IsMain = true;
            }
           await _unitOfWork.SaveChangesAsync();
            
        }
    }
}
