using CloudTrade.Application.Contracts.SalesModes;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.SalesModes;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.SalesModes
{
    public class SalesModeService : BaseService, ISalesModeService
    {
        private readonly ISqlSugarClient db;
        public SalesModeService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }

        public async Task<(IEnumerable<SalesMode>, int)> SalesModeQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<SalesMode>()
.Where(x => x.ModeName.Contains(query));
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
