

namespace CloudTrade.Application.CommodityCompanys
{
    public class CommodityCompanyService:BaseService,ICommodityCompanyService
    {
        private readonly ISqlSugarClient db;
        public CommodityCompanyService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }

        public async Task<(IEnumerable<CommodityCompany>, int)> CommodityCompanyQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "")
        {
            try
            {
                // 验证 PageIndex 和 PageSize 是否有效
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<CommodityCompany>()
                    .Where(x => x.CommodityCompanyName.Contains(query));
                var totalRecords = await queryable.CountAsync();
                var pageCount = (int)Math.Ceiling(totalRecords / (double)PageSize);
                var data = await queryable
                    .Skip((PageIndex - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                return (data, pageCount);
            }
            catch (Exception)
            {
                // 重新抛出异常
                throw;
            }
        }

    }
}
