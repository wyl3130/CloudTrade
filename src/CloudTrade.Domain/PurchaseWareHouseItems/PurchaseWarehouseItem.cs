using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.PurchaseWareHouseItems
{
    [SugarTable(TableName = "PurchaseWareHouseItem", TableDescription = "采购入库项")]
    public class PurchaseWareHouseItem : BaseEntity
    {
        [SugarColumn(IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnName = "CommodityId", ColumnDescription = "商品ID")]
        public Guid CommodityId { get; set; } // 商品ID
        [SugarColumn(IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnName = "PurchaseWareHouseId", ColumnDescription = "关联的采购入库单据Id")]
        public Guid PurchaseWareHouseId { get; set; } // 关联的采购入库单据Id
        [SugarColumn(IsNullable = false, ColumnDataType = "int", ColumnName = "Count", ColumnDescription = "数量")]
        public int Count { get; set; } // 数量
        [SugarColumn(IsNullable = false, ColumnDataType = "float", ColumnName = "Price", ColumnDescription = "商品单价")]
        public float Price { get; set; } // 商品单价
        [SugarColumn(IsNullable = false, ColumnDataType = "float", ColumnName = "Amount", ColumnDescription = "金额")]
        public float Amount { get; set; } // 金额
        [SugarColumn(IsNullable = false, ColumnDataType = "float", ColumnName = "Rebate", ColumnDescription = "返利")]
        public float Rebate { get; set; } // 返利
        [SugarColumn(IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnName = "ModeInfoId", ColumnDescription = "货品销售方式")]
        public Guid ModeInfoId { get; set; } // 货品销售方式
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}

