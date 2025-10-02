namespace PasabuyAPI.DTOs.Requests
{
    public class DeliveryDetailsRequestDTO
    {
        public long DeliveryIdPk { get; set; }
        public long? OrderIdFK { get; set; }
        public decimal EstimatedDistance { get; set; }
        public decimal ActualDistance { get; set; }
        public decimal CourierLatitude { get; set; }
        public decimal CourierLongitude { get; set; }
        public decimal CustomerLatitude { get; set; }
        public decimal CustomerLongitude { get; set; }
        public DateTime EstimatedDeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public decimal DeliveryFee { get; set; }
        public string DeliveryNotes { get; set; } = string.Empty;
    }
}