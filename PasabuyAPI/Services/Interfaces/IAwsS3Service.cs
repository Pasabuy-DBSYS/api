using PasabuyAPI.DTOs.Responses;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IAwsS3Service
    {
        Task<VerificationInfoPathsResponseDTO> UploadIDs(IFormFile frontId, IFormFile backId, IFormFile insurance);
        Task<string> UploadFileAsync(IFormFile file, string key);
        Task<Stream?> GetFileAsync(string targetDirectory, string fileName);
        string GenerateSignedUrl(string key, TimeSpan validFor);
    }
}