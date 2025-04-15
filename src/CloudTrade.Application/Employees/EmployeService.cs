using CloudTrade.Domain.Positions;
using System.Drawing.Printing;

namespace CloudTrade.Application.Employees
{
    public class EmployeService : BaseService, IEmployeService
    {
        private readonly ISqlSugarClient db;
        public EmployeService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }
        public async Task<(IEnumerable<EmployeDto>, int)> EmployeQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string PositionName = "",string DepartmentName = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<Employe, Position, Department>((e, p, d) => new JoinQueryInfos(
        JoinType.Inner, e.PositionId.Equals(p.Id),
        JoinType.Inner, p.DepartmentId.Equals(d.Id)
    ))
    .Where((e, p, d) => e.RealName.Contains(query) && p.PositionName.Contains(PositionName) && d.DepartmentName.Contains(DepartmentName))
    .Select((e, p, d) => new EmployeDto()
    {
        Id = e.Id,
        CreateTime = e.CreateTime,
        LastUpdateTime = e.LastUpdateTime,
        Address = e.Address,
        BornDate = e.BornDate,
        Education = e.Education,
        Email = e.Email,
        Phone = e.Phone,
        PositionId = p.Id,
        PositionName = p.PositionName,
        PostalCode = e.PostalCode,
        DepartmentId = d.Id,
        RealName = e.RealName,
        Remark = e.Remark,
        Salary = e.Salary,
        Sex = e.Sex,
        DepartmentName = d.DepartmentName
    });
                var totalRecords = await queryable.CountAsync();
                var pageCount = (int)Math.Ceiling(totalRecords / (double)PageSize);
                var data = await queryable.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
                return (data, pageCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
