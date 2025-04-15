using CloudTrade.Domain.Commoditys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.Commoditys
{
    public class CommodityDto:Commodity
    {
        public string CommodityCategoryName { get; set; }
        public string CommodityCompanyName { get; set; }
    }
}
