using CloudTrade.Domain.SalesRefunds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.SalesRefunds
{
    public class SalesRefundDto: SalesRefund
    {
        public string CustomerCompanyName { get; set; }
        public string WareHouseName { get; set; }
        public string PaymentAccountName { get; set; }
        public string PaymentCategoryName { get; set; }
    }
}
