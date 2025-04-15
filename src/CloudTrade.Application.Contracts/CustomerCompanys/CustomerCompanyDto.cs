using CloudTrade.Domain.CustomerCompanys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.CustomerCompanys
{
    public class CustomerCompanyDto: CustomerCompany
    {
        public string CustomerCategoryName { get; set; }
    }
}
