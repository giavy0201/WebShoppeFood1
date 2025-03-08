using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IService
{
    public interface IMoMoService
    {
        Task<string> CreateMoMoPaymentUrl(double amount, int orderId, string orderInfo);
        Task HandleMoMoResponse(MoMoPaymentResponseModel response);
    }
}
