using CloudTrade.Domain.PurchaseRefunds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.PurchaseRefunds
{
    public class PurchaseRefundDto: PurchaseRefund
    {
        public string CustomerCompanyName { get; set; }
        public string PaymentCategoryName { get; set; }
        public string PaymentAccountName { get; set; }
        public string WareHouseName { get; set; }

    }
}
