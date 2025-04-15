using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.OtherWareHouseItems
{
    [SugarTable("OtherWareHouseItem", TableDescription = "其他入库项目表")]
    public class OtherWareHouseItem : BaseEntity
    {

        [SugarColumn(ColumnName = "CommodityId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的商品ID")]
        public Guid CommodityId { get; set; } // 关联的商品ID

        [SugarColumn(ColumnName = "OtherWareHouseId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的入库单据id")]
        public Guid OtherWareHouseId { get; set; } // 关联的入库单据id

        [SugarColumn(ColumnName = "Count", IsNullable = false, ColumnDataType = "int", ColumnDescription = "数量")]
        public int Count { get; set; } // 数量

        [SugarColumn(ColumnName = "Price", IsNullable = false, ColumnDataType = "float", ColumnDescription = "商品单价")]
        public double Price { get; set; } // 商品单价

        [SugarColumn(ColumnName = "Amount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "金额")]
        public double Amount { get; set; } // 金额

        [SugarColumn(ColumnName = "TaxPrice", IsNullable = false, ColumnDataType = "float", ColumnDescription = "含税单价")]
        public double TaxPrice { get; set; } // 含税单价

        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "备注")]
        public string Remark { get; set; } // 备注

        [SugarColumn(ColumnName = "Tax", IsNullable = false, ColumnDataType = "float", ColumnDescription = "税率")]
        public double Tax { get; set; } // 税率

        [SugarColumn(ColumnName = "TaxAmount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "含税金额")]
        public double TaxAmount { get; set; } // 含税金额
    }

}
