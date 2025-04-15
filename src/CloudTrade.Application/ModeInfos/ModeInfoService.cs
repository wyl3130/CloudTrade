using CloudTrade.Application.Contracts.ModeInfos;
using CloudTrade.Domain.ModeInfos;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CloudTrade.Application.ModeInfos
{
    public class ModeInfoService : BaseService, IModeInfoService
    {
        private readonly ISqlSugarClient db;
        public ModeInfoService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }

        public async Task<(IEnumerable<ModeInfo>, int)> ModeInfoQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<ModeInfo>()
.Where(x => x.ModeInfoName.Contains(query));
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
