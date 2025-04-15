using CloudTrade.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.Payments
{
    public class PaymentDto: Payment
    {
        public string CustomerCompanyName { get; set; }
        public string PaymentCategoryName { get; set; }
        public string PaymentAccountName { get; set; }
    }
}
