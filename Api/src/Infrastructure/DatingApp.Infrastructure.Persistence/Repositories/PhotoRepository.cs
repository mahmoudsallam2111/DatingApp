using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.Application.Abstractions;
using DatingApp.Application.Helpers;
using DatingApp.Application.Interfaces.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace DatingApp.Infrastructure.Persistence.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        public Task<ImageUploadResult> AddPhotoAsync(IFile formFile)
        {
            throw new NotImplementedException();
        }

        public Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            throw new NotImplementedException();
        }
    }
}
