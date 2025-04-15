using CloudTrade.Domain.Receipts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.Receipts
{
    public class ReceiptDto: Receipt
    {
        public string CustomerCompanyName { get; set; }
        public string PaymentCategoryName { get; set; }
        public string PaymentAccountName { get; set; }

    }
}
