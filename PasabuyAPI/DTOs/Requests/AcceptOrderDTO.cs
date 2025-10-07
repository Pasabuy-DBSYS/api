namespace PasabuyAPI.DTOs.Requests
{
    public class AcceptOrderDTO
    {
        public long CourierId { get; set; }
        public decimal CourierLatitude { get; set; }
        public decimal CourierLongitude { get; set; }
    }
}