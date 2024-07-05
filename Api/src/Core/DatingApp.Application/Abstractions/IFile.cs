namespace DatingApp.Application.Abstractions
{
    public interface IFile
    {
        string FileName { get; }
        string Name { get; }
        string ContentDisposition { get; }
        string ContentType { get; }
        long Length { get; }

        Stream OpenReadStream();
        void CopyTo(Stream target);
        Task CopyToAsync(Stream target, CancellationToken cancellationToken = default(CancellationToken));


    }
}
