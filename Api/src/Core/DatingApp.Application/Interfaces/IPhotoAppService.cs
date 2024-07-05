using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Application.Interfaces
{
    public interface IPhotoAppService
    {
        Task<ImageUploadResult> AddPhoto(IFormFile formFile , string userName);
        Task<DeletionResult> DeletePhoto(string publicId);
        Task SetUserMainPhoto(int photoId ,  string userName);
        Task DeletePhotoAsync(int photoId , string userName);
    }
}
