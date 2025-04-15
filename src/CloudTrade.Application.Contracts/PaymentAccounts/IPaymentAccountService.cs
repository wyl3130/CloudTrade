using CloudTrade.Domain.Departments;
using CloudTrade.Domain.PaymentAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.PaymentAccounts
{
    public  interface IPaymentAccountService:IBaseService
    {
        Task<(IEnumerable<PaymentAccount>, int)> PaymentAccountQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "");
    }
}
