namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IChatRoomRepository
    {
        Task<bool> CloseChatRoomAsync(long roomId, long currentUserId);
    }
}