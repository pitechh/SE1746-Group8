namespace FLearning.PaymentService.Models.DTOs
{
    public class PaymentCallbackRequestDTO
    {
        public string OrderId { get; set; }
        public string Amount { get; set; }
        public string OrderInfo { get; set; }
    }
}
