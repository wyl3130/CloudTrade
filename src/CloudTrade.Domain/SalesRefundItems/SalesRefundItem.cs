using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.SalesRefundItems
{
    [SugarTable("SalesRefundItem", TableDescription = "销售退货单明细")]
    public class SalesRefundItem : BaseEntity
    {

        [SugarColumn(ColumnName = "CommodityId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的商品ID")]
        public Guid CommodityId { get; set; } // 关联的商品ID

        [SugarColumn(ColumnName = "SalesRefundId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的销售退货单据Id")]
        public Guid SalesRefundId { get; set; } // 关联的销售退货单据Id

        [SugarColumn(ColumnName = "Count", IsNullable = false, ColumnDataType = "int", ColumnDescription = "数量")]
        public int Count { get; set; } // 数量

        [SugarColumn(ColumnName = "Price", IsNullable = false, ColumnDataType = "float", ColumnDescription = "商品单价")]
        public double Price { get; set; } // 商品单价

        [SugarColumn(ColumnName = "Amount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "金额")]
        public double Amount { get; set; } // 金额

        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "备注")]
        public string Remark { get; set; } // 备注
    }

}
