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
        [SugarColumn(ColumnName = "Id", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "编号")]
        public Guid Id { get; set; } // 编号

        [SugarColumn(ColumnName = "CreateTime", IsNullable = false, ColumnDataType = "nvarchar", Length = 100, ColumnDescription = "创建时间")]
        public string CreateTime { get; set; } // 创建时间

        [SugarColumn(ColumnName = "LastUpdateTime", IsNullable = false, ColumnDataType = "nvarchar", Length = 100, ColumnDescription = "最后修改时间")]
        public string LastUpdateTime { get; set; } // 最后修改时间

        [SugarColumn(ColumnName = "CommodityId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的商品id")]
        public Guid CommodityId { get; set; } // 关联的商品id

        [SugarColumn(ColumnName = "SalesOrderId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的销售单据Id")]
        public Guid SalesOrderId { get; set; } // 关联的销售单据Id

        [SugarColumn(ColumnName = "Amount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "金额")]
        public double Amount { get; set; } // 金额

        [SugarColumn(ColumnName = "TaxAmount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "含税金额")]
        public double TaxAmount { get; set; } // 含税金额

        [SugarColumn(ColumnName = "TaxPrice", IsNullable = false, ColumnDataType = "float", ColumnDescription = "含税单价")]
        public double TaxPrice { get; set; } // 含税单价

        [SugarColumn(ColumnName = "Price", IsNullable = false, ColumnDataType = "float", ColumnDescription = "商品单价")]
        public double Price { get; set; } // 商品单价

        [SugarColumn(ColumnName = "Tax", IsNullable = false, ColumnDataType = "float", ColumnDescription = "税率")]
        public double Tax { get; set; } // 税率

        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "备注")]
        public string Remark { get; set; } // 备注

        [SugarColumn(ColumnName = "Count", IsNullable = false, ColumnDataType = "int", ColumnDescription = "数量")]
        public int Count { get; set; } // 数量

        [SugarColumn(ColumnName = "Rebate", IsNullable = false, ColumnDataType = "float", ColumnDescription = "返利")]
        public double Rebate { get; set; } // 返利

        [SugarColumn(ColumnName = "ModeInfoId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "货品销售方式")]
        public Guid ModeInfoId { get; set; } // 货品销售方式
    }

}
