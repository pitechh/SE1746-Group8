namespace FLearning.PaymentService.Models.DTOs
{
    public class OrderCreateDTO
    {
        public Guid StudentId { get; set; }
        public int CourseId { get; set; }
        public string FullName { get; set; }
        public string OrderInfo { get; set; }
        public double Amount { get; set; }
    }
}
