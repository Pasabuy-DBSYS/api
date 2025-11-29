using Mapster;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class VerificationInfoService(IVerificationInfoRepository verificationInfoRepository, IAwsS3Service awsS3Service, IEmailServices emailServices, IUserService userService, IWebHostEnvironment env) : IVerificationInfoService
    {
        public async Task<VerificationInfoResponseDTO> CreateVerificationInfo(VerificationInfoRequestDTO requestDTO)
        {
            VerificationInfo verificationInfo = await verificationInfoRepository.CreateVerificationInfoAsync(requestDTO.Adapt<VerificationInfo>());
            return verificationInfo.Adapt<VerificationInfoResponseDTO>();
        }

        public async Task<VerificationInfoResponseDTO> UpdateVerificationInfoByUserIdAsync(VerificationInfoStatus verificationInfoStatus, long userId)
        {
            VerificationInfo verificationInfo = await verificationInfoRepository.UpdateVerificationInfoByUserIdAsync(verificationInfoStatus, userId);

            UserResponseDTO target = await userService.GetUserByIdAsync(userId);

            var templatePath = Path.Combine(env.ContentRootPath, "EmailTemplates", "VerificationStatus.html");
            var htmlBody = await File.ReadAllTextAsync(templatePath);


            htmlBody = htmlBody
                .Replace("{{user}}", $"{target.FirstName}")
                .Replace("{{appName}}", "PasaBuy")
                .Replace("{{status}}", verificationInfoStatus.ToString())
                .Replace("{{reviewDate}}", verificationInfo.UpdatedAt.ToString("MMMM dd, yyyy"))
                .Replace("{{companyName}}", "PasaBuy")
                .Replace("{{supportUrl}}", "pasabuy.application@gmail.com")
                .Replace("{{companyAddress}}", "Cebu City, Cebu, Philippines");

            switch (verificationInfoStatus)
            {
                case VerificationInfoStatus.ACCEPTED:
                    htmlBody = htmlBody.Replace("{{message}}", "Congratulations! Your verification has been approved. You now have full access to all features of your PasaBuy account.");
                    break;
                
                case VerificationInfoStatus.REJECTED:
                    htmlBody = htmlBody.Replace("{{message}}", "Unfortunately, your verification was not approved at this time. If you believe this was an error or would like to resubmit, please contact our support team.");
                    break;
                
                case VerificationInfoStatus.PENDING:
                    htmlBody = htmlBody.Replace("{{message}}", "Your verification is currently under review. We'll notify you once a decision has been made.");
                    break;
                
                default:
                    htmlBody = htmlBody.Replace("{{message}}", "There has been an update to your verification status.");
                    break;
            }

            await emailServices.SendEmailAsync(target.Email, "[PASABUY] Verification Status Update", htmlBody);

            return verificationInfo.Adapt<VerificationInfoResponseDTO>();
        }

        public async Task<VerificationInfoResponseDTO> UpdateInsuranceAsync(IFormFile insurance, long userId)
        {
            string key = $"ids/insurance_{Guid.NewGuid()}";

            var path = await awsS3Service.UploadFileAsync(insurance, key);

            VerificationInfo verificationInfo = await verificationInfoRepository.UpdateInsuranceAsync(path, userId);

            return verificationInfo.Adapt<VerificationInfoResponseDTO>();
        }
    }
}