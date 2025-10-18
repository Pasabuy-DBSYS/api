using Amazon.S3;
using Amazon.S3.Model;
using Amazon.CloudFront;
using Amazon.CloudFront.Model;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Exceptions;
using PasabuyAPI.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;


namespace PasabuyAPI.Services.Implementations
{
    public class AwsS3Service(IAmazonS3 s3Client, IConfiguration config) : IAwsS3Service
    {
        
        private readonly string _bucketName = config["AWS:BucketName"]
            ?? throw new NotFoundException("Configuration 'AWS:BucketName' is not found");

        private readonly string _cloudFrontDomain = config["AWS:CloudFrontDomain"]
            ?? throw new NotFoundException("Configuration 'AWS:CloudFrontDomain' is not found");

        private readonly string _keyPairId = config["AWS:CloudFrontKeyPairId"]
            ?? throw new NotFoundException("Configuration 'AWS:CloudFrontKeyPairId' is not found");

        private readonly string _privateKeyPath = config["AWS:CloudFrontPrivateKeyPath"]
            ?? throw new NotFoundException("Configuration 'AWS:CloudFrontPrivateKeyPath' is not found");
        public async Task<VerificationInfoPathsResponseDTO> UploadIDs(IFormFile frontId, IFormFile backId, IFormFile insurance)
        {
            var frontKey = $"front_{Guid.NewGuid()}";
            var backKey = $"back_{Guid.NewGuid()}";
            var insuranceKey = $"insurance_{Guid.NewGuid()}";

            // Upload each file
            await UploadFileAsync(frontId, $"ids/{frontKey}");
            await UploadFileAsync(backId, $"ids/{backKey}");
            await UploadFileAsync(insurance, $"ids/{insuranceKey}");

            return new VerificationInfoPathsResponseDTO
            {
                FrontIdFileName = frontKey,
                BackIdFileName = backKey,
                InsuranceFileName = insuranceKey
            };
        }

        public async Task<string> UploadFileAsync(IFormFile file, string key)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType
            };
            await s3Client.PutObjectAsync(request);
            return key;
        }

        public Task<Stream?> GetFileAsync(string targetDirectory, string fileName)
        {
            throw new NotImplementedException();
        }
        public string GenerateSignedUrl(string key, TimeSpan validFor)
        {
            var resourceUrl = $"{_cloudFrontDomain.TrimEnd('/')}/{key}";
            var expiresOn = DateTimeOffset.UtcNow.Add(validFor).ToUnixTimeSeconds();

            // Create policy
            var policy = $@"{{""Statement"":[{{""Resource"":""{resourceUrl}"",""Condition"":{{""DateLessThan"":{{""AWS:EpochTime"":{expiresOn}}}}}}}]}}";
            
            // Load and parse private key
            var privateKeyText = File.ReadAllText(_privateKeyPath);
            using var rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyText);

            // Sign the policy
            var policyBytes = Encoding.UTF8.GetBytes(policy);
            var signatureBytes = rsa.SignData(policyBytes, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            
            // Base64 encode and make URL-safe
            var signature = Convert.ToBase64String(signatureBytes)
                .Replace("+", "-")
                .Replace("=", "_")
                .Replace("/", "~");
                
            var encodedPolicy = Convert.ToBase64String(policyBytes)
                .Replace("+", "-")
                .Replace("=", "_")
                .Replace("/", "~");

            return $"{resourceUrl}?Policy={encodedPolicy}&Signature={signature}&Key-Pair-Id={_keyPairId}";
        }

        public async Task DeleteFileAsync(string key)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            await s3Client.DeleteObjectAsync(request);
        }

    }
}