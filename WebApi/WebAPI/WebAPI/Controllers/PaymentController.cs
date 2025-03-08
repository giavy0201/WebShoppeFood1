using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using BLL.Models.Request;
using BLL.Models;
using BLL.Service;
namespace WebAPI.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IMoMoService _moMoService;


        public PaymentController(IMoMoService moMoService)
        {
            _moMoService = moMoService;
            //_paymentService = paymentService;
        }

        [HttpPost("Payment/create-payment-url")]
        public async Task<IActionResult> CreatePaymentUrl([FromBody] PaymentRequest request)
        {
            try
            {
                string paymentUrl = await _moMoService.CreateMoMoPaymentUrl(
                 request.Amount,
                 request.OrderId,
                 request.OrderInfo
             );
                //if (request.Amount <= 0)
                //{
                //    // Handle invalid Amount
                //    throw new ArgumentException("Amount must be greater than zero", nameof(request.Amount));
                //}

                //if (string.IsNullOrWhiteSpace(request.OrderId.ToString()))
                //{
                //    // Handle missing OrderId
                //    throw new ArgumentException("OrderId cannot be null or empty", nameof(request.OrderId));
                //}

                //if (string.IsNullOrWhiteSpace(request.OrderInfo))
                //{
                //    // Handle missing OrderInfo
                //    throw new ArgumentException("OrderInfo cannot be null or empty", nameof(request.OrderInfo));
                //}

                return Ok(new { PayUrl = paymentUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("Payment/callback")]
        public async Task<IActionResult> HandlePaymentResponse([FromQuery] string partnerCode,
    [FromQuery] string orderId,
    [FromQuery] string requestId,
    [FromQuery] string amount,
    [FromQuery] string orderInfo,
    [FromQuery] string orderType,
    [FromQuery] string transId,
    [FromQuery] string resultCode,
    [FromQuery] string message,
    [FromQuery] string signature,
    [FromQuery] string responseTime,
    [FromQuery] string extraData)
        {
            // Kiểm tra định dạng orderId
            if (!int.TryParse(orderId, out int parsedOrderId))
            {
                return BadRequest("Invalid orderId format.");
            }

            // Kiểm tra định dạng amount
            if (!double.TryParse(amount, out double parsedAmount))
            {
                return BadRequest("Invalid amount format.");
            }

            // Kiểm tra định dạng responseTime và chuyển thành long
            if (!long.TryParse(responseTime, out long parsedResponseTime))
            {
                return BadRequest("Invalid responseTime format.");
            }

            // Kiểm tra và chuyển đổi resultCode thành int
            int parsedResultCode = -1; // Mặc định là -1 nếu không thể chuyển
            if (!int.TryParse(resultCode, out parsedResultCode))
            {
                return BadRequest("Invalid resultCode format.");
            }

            // Tạo đối tượng MoMoPaymentResponseModel với các giá trị đã kiểm tra
            var responseModel = new MoMoPaymentResponseModel
            {
                PartnerCode = partnerCode,
                OrderId = parsedOrderId,
                RequestId = requestId,
                Amount = parsedAmount,
                OrderInfo = orderInfo,
                OrderType = orderType,
                TransId = transId,
                ResultCode = parsedResultCode,
                Message = message,
                Signature = signature,
                ResponseTime = parsedResponseTime,
                ExtraData = extraData
            };

            try
            {
                // Xử lý callback thông qua MoMoService
                await _moMoService.HandleMoMoResponse(responseModel);

                // Phản hồi thành công cho MoMo
                return Redirect("https://localhost:7203/Cart/History");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest(new { message = "Payment callback failed", error = ex.Message });
            }
            }
         }
    }
    

