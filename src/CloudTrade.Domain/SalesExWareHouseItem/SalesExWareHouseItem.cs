using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.SalesExWareHouseItem
{
    [SugarTable("SalesExWareHouseItem", TableDescription = "销售出库单据项")]
    public class SalesExWareHouseItem : BaseEntity
    {

        [SugarColumn(ColumnName = "CommodityId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的商品id")]
        public Guid CommodityId { get; set; } // 关联的商品id

        [SugarColumn(ColumnName = "SalesExWareHouseId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的销售出库单据Id")]
        public Guid SalesExWareHouseId { get; set; } // 关联的销售出库单据Id

        [SugarColumn(ColumnName = "Price", IsNullable = false, ColumnDataType = "float", ColumnDescription = "商品单价")]
        public float Price { get; set; } // 商品单价

        [SugarColumn(ColumnName = "Count", IsNullable = false, ColumnDataType = "int", ColumnDescription = "数量")]
        public int Count { get; set; } // 数量

        [SugarColumn(ColumnName = "Amount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "金额")]
        public float Amount { get; set; } // 金额

        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "备注")]
        public string Remark { get; set; } // 备注

        [SugarColumn(ColumnName = "Rebate", IsNullable = false, ColumnDataType = "float", ColumnDescription = "返利")]
        public float Rebate { get; set; } // 返利

        [SugarColumn(ColumnName = "ModeInfoId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "货品销售方式")]
        public Guid ModeInfoId { get; set; } // 货品销售方式
    }
}
