using PasabuyAPI.Enums;

namespace PasabuyAPI.DTOs.Requests
{
    public class CreateOrderDTO

    {
        public long CustomerId { get; set; }
        public string Request { get; set; } = string.Empty;
        public decimal? TipFee { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public decimal LocationLatitude { get; set; }
        public decimal LocationLongitude { get; set; }
        public decimal CustomerLatitude { get; set; }
        public decimal CustomerLongitude { get; set; }
        public decimal DeliveryDistance { get; set; }
        public string DeliveryNotes { get; set; } = string.Empty;
    }
}