using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.WareHouseStocktakingItems
{
    [SugarTable(TableName = "WareHouseStocktakingItem", TableDescription = "仓库盘点子项表")]

    public class WareHouseStocktakingItem:BaseEntity
    {
        [SugarColumn(ColumnName = "", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "编号")]
        public Guid CommodityId { get; set; }
        [SugarColumn(ColumnName = "", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "编号")]
        public Guid WareHouseStocktakingId { get; set; }
        [SugarColumn(ColumnName = "Count", IsNullable = false, ColumnDataType = "int", ColumnDescription = "数量")]
        public int Count { get; set; }
        [SugarColumn(ColumnName = "CurrentCount", IsNullable = false, ColumnDataType = "int", ColumnDescription = "实际数量")]
        public int CurrentCount { get; set; }
        [SugarColumn(ColumnName = "Price", IsNullable = false, ColumnDataType = "float", ColumnDescription = "单价")]
        public double Price { get; set; }
        [SugarColumn(ColumnName = "Amount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "金额")]
        public double Amount { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
