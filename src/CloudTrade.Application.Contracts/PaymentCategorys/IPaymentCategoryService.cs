using CloudTrade.Domain.Departments;
using CloudTrade.Domain.PaymentCategorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.PaymentCategorys
{
    public interface IPaymentCategoryService:IBaseService
    {
        Task<(IEnumerable<PaymentCategory>, int)> PaymentCategoryQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "");
    }
}
