using CloudTrade.Domain.OtherFinances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.OtherFinances
{
    public class OtherFinanceDto: OtherFinance
    {
        public string DepositAccountName { get; set; }
        public string FinanceCategoryName { get; set; }
        public string PaymentCategoryName { get; set; }
    }
}
