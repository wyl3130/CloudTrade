namespace CloudTrade.Application.CommodityStocks
{
    public class CommodityStockService : BaseService, ICommodityStockService
    {
        private readonly ISqlSugarClient db;
        public CommodityStockService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }
        public async Task<(IEnumerable<CommodityStockDto>, int)> CommodityStockQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string WareHouseName = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<CommodityStock, Commodity, WareHouse>((cs, c, w) => new JoinQueryInfos(
    JoinType.Inner, cs.CommodityId.Equals(c.Id),
    JoinType.Inner, w.Id.Equals(cs.WareHouseId)
    ))
    .Where((cs, c, w) => c.CommodityName.Contains(query) && w.WareHouseName.Contains(WareHouseName))
    .Select((cs, c, w) => new CommodityStockDto()
    {
        Id = cs.Id,
        CommodityId = c.Id,
        WareHouseId = w.Id,
        CommodityName = c.CommodityName,
        WareHouseName = w.WareHouseName,
        CreateTime = cs.CreateTime,
        LastUpdateTime = cs.LastUpdateTime,
        SalesCount = cs.SalesCount,
        StockCount = cs.StockCount

    });
                var totalRecords = await queryable.CountAsync();
                var pageCount = (int)Math.Ceiling(totalRecords / (double)PageSize);
                var data = await queryable.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
                return (data, pageCount);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
