namespace PasabuyAPI.DTOs.Responses
{
    public class DeliveryDetailsResponseDTO
    {
        public long DeliveryIdPk { get; set; }
        public long OrderIdFK { get; set; }
        public decimal ActualDistance { get; set; }
        public decimal LocationLongitude { get; set; }
        public decimal LocationLatitude { get; set; }
        public decimal CourierLatitude { get; set; }
        public decimal CourierLongitude { get; set; }
        public decimal CustomerLatitude { get; set; }
        public decimal CustomerLongitude { get; set; }
        public string DestinationAddress { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public DateTime EstimatedDeliveryTime { get; set; }
        public DateTime ActualDeliveryTime { get; set; }
        public DateTime ActualPickupTime { get; set; }
        public string DeliveryNotes { get; set; } = string.Empty;
    }
}