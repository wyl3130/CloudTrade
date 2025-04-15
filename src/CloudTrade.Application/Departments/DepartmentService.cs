
using System.Drawing.Printing;

namespace CloudTrade.Application.Departments
{
    public class DepartmentService : BaseService, IDepartmentService
    {
        private readonly ISqlSugarClient db;
        public DepartmentService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }

        public async Task<(IEnumerable<Department>, int)> DepartmentQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<Department>()
    .Where(x => x.DepartmentName.Contains(query));
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
