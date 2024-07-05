using CloudinaryDotNet.Actions;
using DatingApp.Application.Abstractions;

namespace DatingApp.Application.Interfaces.Repositories
{
    public interface IPhotoRepository
    {
        Task<ImageUploadResult> AddPhotoAsync(IFile formFile);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
