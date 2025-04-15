using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.PurchaseOrderItems
{
    [SugarTable(TableName = "PurchaseOrderItem", TableDescription = "采购订单 项")]
    public class PurchaseOrderItem:BaseEntity
    {
        [SugarColumn(ColumnName = "PurchaseOrderId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "采购订单编号")]
        public Guid PurchaseOrderId { get; set; }
        [SugarColumn(ColumnName = "CommodityId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "商品编号")]
        public Guid CommodityId { get; set; }
        [SugarColumn(ColumnName = "Count", IsNullable = false, ColumnDataType = "int", ColumnDescription = "数量")]
        public int Count { get; set; }
        [SugarColumn(ColumnName = "Price", IsNullable = false, ColumnDataType = "float", ColumnDescription = "单价")]
        public float Price { get; set; }
        [SugarColumn(ColumnName = "Amount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "金额")]
        public float Amount { get; set; }
        [SugarColumn(ColumnName = "TaxPrice", IsNullable = false, ColumnDataType = "float", ColumnDescription = "含税单价")]
        public float TaxPrice { get; set; }
        [SugarColumn(ColumnName = "Tax", IsNullable = false, ColumnDataType = "float", ColumnDescription = "税率")]
        public float Tax { get; set; }
        [SugarColumn(ColumnName = "TaxAmount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "含税金额")]
        public float TaxAmount { get; set; }
        [SugarColumn(ColumnName = "Rebate", IsNullable = false, ColumnDataType = "float", ColumnDescription = "返利")]
        public float Rebate { get; set; }
        [SugarColumn(ColumnName = "ModeInfoId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "销售方式编号")]
        public Guid ModeInfoId { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
