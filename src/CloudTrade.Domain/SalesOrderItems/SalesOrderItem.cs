using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.SalesOrderItems
{
    [SugarTable("SalesOrderItem", TableDescription = "销售订单商品项")]
    public class SalesOrderItem : BaseEntity
    {


        [SugarColumn(ColumnName = "CommodityId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的商品id")]
        public Guid CommodityId { get; set; } // 关联的商品id

        [SugarColumn(ColumnName = "SalesOrderId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的销售单据Id")]
        public Guid SalesOrderId { get; set; } // 关联的销售单据Id

        [SugarColumn(ColumnName = "Amount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "金额")]
        public float Amount { get; set; } // 金额

        [SugarColumn(ColumnName = "TaxAmount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "含税金额")]
        public float TaxAmount { get; set; } // 含税金额

        [SugarColumn(ColumnName = "TaxPrice", IsNullable = false, ColumnDataType = "float", ColumnDescription = "含税单价")]
        public float TaxPrice { get; set; } // 含税单价

        [SugarColumn(ColumnName = "Price", IsNullable = false, ColumnDataType = "float", ColumnDescription = "商品单价")]
        public float Price { get; set; } // 商品单价

        [SugarColumn(ColumnName = "Tax", IsNullable = false, ColumnDataType = "float", ColumnDescription = "税率")]
        public float Tax { get; set; } // 税率

        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "备注")]
        public string Remark { get; set; } // 备注

        [SugarColumn(ColumnName = "Count", IsNullable = false, ColumnDataType = "int", ColumnDescription = "数量")]
        public int Count { get; set; } // 数量

        [SugarColumn(ColumnName = "Rebate", IsNullable = false, ColumnDataType = "float", ColumnDescription = "返利")]
        public float Rebate { get; set; } // 返利

        [SugarColumn(ColumnName = "ModeInfoId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "货品销售方式")]
        public Guid ModeInfoId { get; set; } // 货品销售方式
    }

}
