





namespace CloudTrade.Application.CommodityCategorys
{
    public class CommodityCategoryService : BaseService, ICommodityCategoryService
    {
        private readonly ISqlSugarClient db;
        public CommodityCategoryService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }

        public async Task<(IEnumerable<CommodityCategory>, int)> CommodityCategoryQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "")
        {
            try
            {                // 验证 PageIndex 和 PageSize 是否有效
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                // 构建查询
                var queryable = db.Queryable<CommodityCategory>()
                    .Where(x => x.CommodityCategoryName.Contains(query));

                // 获取总记录数
                var totalRecords = await queryable.CountAsync();

                // 计算总页数，根据传入的PageSize
                var pageCount = (int)Math.Ceiling(totalRecords / (double)PageSize);

                // 分页查询数据
                var data = await queryable
                    .Skip((PageIndex - 1) * PageSize)  // 跳过前面已显示的记录
                    .Take(PageSize)                    // 获取当前页的记录数
                    .ToListAsync();

                // 返回分页结果
                return (data, pageCount);
            }
            catch (Exception ex)
            {
                // 记录日志或者抛出自定义异常，避免直接抛出原始异常
                // 比如可以用日志记录：_logger.LogError(ex, "Failed to query commodity categories.");
                throw new Exception("An error occurred while fetching commodity categories.", ex);
            }
        }

    }
}
