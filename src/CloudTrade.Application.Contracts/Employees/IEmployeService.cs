using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.Employees
{
    public interface IEmployeService:IBaseService
    {
        Task<(IEnumerable<EmployeDto>, int)> EmployeQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string PositionName = "",string DepartmentName = "");
    }
}
