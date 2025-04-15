using CloudTrade.Domain.CustomerCategorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.CustomerCompanys
{
    public interface ICustomerCompanyService:IBaseService
    {
        Task<(IEnumerable<CustomerCompanyDto>, int)> CustomerCompanyQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "");
    }
}
