using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.CommodityStocks
{
    [SugarTable(TableName = "CommodityStock", TableDescription = "库存表")]
    public class CommodityStock:BaseEntity
    {
        [SugarColumn(ColumnName = "WareHouseId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "仓库编号")]
        public Guid WareHouseId { get; set; }
        [SugarColumn(ColumnName = "CommodityId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "商品编号")]
        public Guid CommodityId { get; set; }
        [SugarColumn(ColumnName = "StockCount", IsNullable = false, ColumnDataType = "int", ColumnDescription = "库存数量")]
        public int StockCount { get; set; }
        [SugarColumn(ColumnName = "SalesCount", IsNullable = false, ColumnDataType = "int", ColumnDescription = "出库数量")]
        public int SalesCount { get; set; }
    }
}
