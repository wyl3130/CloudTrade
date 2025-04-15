


using CloudTrade.Domain.Positions;
using System.Drawing.Printing;

namespace CloudTrade.Application.Positions
{
    public class PositionService : BaseService, IPositionService
    {
        private readonly ISqlSugarClient db;
        public PositionService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }


        public async Task<(IEnumerable<PositionDto>, int )> PositionQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string DepartmentName = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<Position, Department>((p, d) => new JoinQueryInfos(
        JoinType.Inner, p.DepartmentId.Equals(d.Id)
    ))
    .Where((p, d) => p.PositionName.Contains(query) && d.DepartmentName.Contains(DepartmentName))
    .Select((p, d) => new PositionDto()
    {
        Id = p.Id,
        CreateTime = p.CreateTime,
        LastUpdateTime = p.LastUpdateTime,
        DepartmentId = d.Id,
        DepartmentName = d.DepartmentName,
        PositionName = p.PositionName,
        Remark = p.Remark
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
