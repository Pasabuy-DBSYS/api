namespace PasabuyAPI.DTOs.Requests
{
    public class ProposePaymentRequestDTO
    {
        public long OrderIdFK { get; set; }
        public decimal ItemsFee { get; set; }
        public IFormFile Image { get; set; } = null!;
    }
}