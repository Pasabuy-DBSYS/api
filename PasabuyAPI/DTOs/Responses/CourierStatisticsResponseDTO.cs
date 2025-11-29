namespace PasabuyAPI.DTOs.Responses
{
    public class CourierStatisticsResponseDTO
    {
        public long UserId {get;set;}
        public long TotalDeliveries {get;set;}
        public decimal TotalEarnings {get;set;}
        public decimal Rating {get;set;}
    }
}