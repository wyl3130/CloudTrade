﻿using CloudTrade.Application.Contracts.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.Receipts
{
    public interface IReceiptService:IBaseService
    {
        Task<(IEnumerable<ReceiptDto>, int )> ReceiptQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string CustomerCompanyName = "");
    }
}
