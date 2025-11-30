using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Models;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IReviewsService
    {
        Task<ReviewResponseDTO> CreateReviewAsync(CreateReviewRequestDTO reviewDto, long reviewerId);
        Task<List<ReviewResponseDTO>> GetAllReviewsAsync();
        Task<ReviewResponseDTO?> GetReviewByIdAsync(long id);
    }
}