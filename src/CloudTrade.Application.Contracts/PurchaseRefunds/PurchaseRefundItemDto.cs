﻿using CloudTrade.Domain.PurchaseRefundItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.PurchaseRefunds
{
    public class PurchaseRefundItemDto:PurchaseRefundItem
    {
        public string CommodityName { get; set; }
    }
}
