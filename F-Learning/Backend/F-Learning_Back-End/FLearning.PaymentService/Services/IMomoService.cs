using FLearning.PaymentService.Models.MoMo;

namespace FLearning.PaymentService.Services
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model);
        Task<MomoExecuteResponseModel> PaymentExecuteAsync(Dictionary<string, string> queryParams);
    }
}
