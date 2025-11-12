namespace PasabuyAPI.DTOs.Requests
{
    public class ChangeProfilePictureRequestDTO
    {
        public IFormFile ProfilePicture { get; set; } = null!;
    }
}