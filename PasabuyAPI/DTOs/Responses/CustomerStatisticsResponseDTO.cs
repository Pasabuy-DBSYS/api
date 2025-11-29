namespace PasabuyAPI.DTOs.Responses
{
    public class CustomerStatisticsResponseDTO
    {
        public long UserId {get;set;}
        public long TotalOrders {get;set;}
        public decimal TotalSpent {get;set;}
        public decimal TotalRating {get;set;}
    }
}