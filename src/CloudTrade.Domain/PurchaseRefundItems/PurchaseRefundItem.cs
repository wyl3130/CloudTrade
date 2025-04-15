using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.PurchaseRefundItems
{
    [SugarTable(TableName = "PurchaseRefundItem", TableDescription = "采购退货 项")]
    public class PurchaseRefundItem:BaseEntity
    {
        [SugarColumn(ColumnName = "PurchaseRefundId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "采购退货编号")]
        public Guid PurchaseRefundId { get; set; }
        [SugarColumn(ColumnName = "CommodityId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "商品编号")]
        public Guid CommodityId { get; set; }
        [SugarColumn(ColumnName = "Count", IsNullable = false, ColumnDataType = "int", ColumnDescription = "数量")]
        public int Count { get; set; }
        [SugarColumn(ColumnName = "Price", IsNullable = false, ColumnDataType = "float", ColumnDescription = "单价")]
        public double Price { get; set; }
        [SugarColumn(ColumnName = "Amount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "金额")]
        public double Amount { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
