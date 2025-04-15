namespace CloudTrade.Application.Commoditys
{
    public class CommodityService : BaseService, ICommodityService
    {
        private readonly ISqlSugarClient db;
        public CommodityService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }

        public async Task<(IEnumerable<CommodityDto>, int)> CommodityQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string CommodityCategoryName = "", string CommodityCompanyName = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<Commodity, CommodityCategory, CommodityCompany>((c, ca, co) => new JoinQueryInfos(
                    JoinType.Inner, c.CommodityCategoryId.Equals(ca.Id),
                    JoinType.Inner, c.CommodityCompanyId.Equals(co.Id)
                    ))
                    .Where((c, ca, co) => c.CommodityName.Contains(query) && ca.CommodityCategoryName.Contains(CommodityCategoryName) && co.CommodityCompanyName.Contains(CommodityCompanyName))
                    .Select((c, ca, co) => new CommodityDto()
                    {
                        Id = c.Id,
                        CodeNo = c.CodeNo,
                        BarCode = c.BarCode,
                        CommodityCategoryName = ca.CommodityCategoryName,
                        CommodityCompanyName = co.CommodityCompanyName,
                        CommodityName = c.CommodityName,
                        CreateTime = c.CreateTime,
                        LastUpdateTime = c.LastUpdateTime,
                        MaxStockCount = c.MaxStockCount,
                        MinStockCount = c.MinStockCount,
                        Ppecification = c.Ppecification,
                        Price = c.Price,
                        PurchasePrice = c.PurchasePrice,
                        Remark = c.Remark,
                        Sort = c.Sort,
                        CommodityCategoryId = c.CommodityCategoryId,
                        CommodityCompanyId = c.CommodityCompanyId,
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
