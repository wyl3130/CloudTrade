using CloudTrade.Domain.WareHouseAllocations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.WareHouseAllocationItems
{
    [SugarTable("WareHouseAllocationItem", TableDescription = "仓库调拨项")]
    public class WareHouseAllocationItem : BaseEntity
    {

        [SugarColumn(ColumnName = "CommodityId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的商品ID")]
        public Guid CommodityId { get; set; } // 关联的商品ID

        [SugarColumn(ColumnName = "WareHouseAllocationId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的仓库调拨单据Id")]
        public Guid WareHouseAllocationId { get; set; } // 关联的仓库调拨单据Id

        [SugarColumn(ColumnName = "Count", IsNullable = false, ColumnDataType = "int", ColumnDescription = "数量")]
        public int Count { get; set; } // 数量

        [SugarColumn(ColumnName = "Price", IsNullable = false, ColumnDataType = "float", ColumnDescription = "商品单价")]
        public float Price { get; set; } // 商品单价

        [SugarColumn(ColumnName = "Amount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "金额")]
        public float Amount { get; set; } // 金额

        [SugarColumn(ColumnName = "TaxPrice", IsNullable = false, ColumnDataType = "float", ColumnDescription = "含税单价")]
        public float TaxPrice { get; set; } // 含税单价

        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "备注")]
        public string Remark { get; set; } // 备注

        [SugarColumn(ColumnName = "Tax", IsNullable = false, ColumnDataType = "float", ColumnDescription = "税率")]
        public float Tax { get; set; } // 税率

        [SugarColumn(ColumnName = "TaxAmount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "含税金额")]
        public float TaxAmount { get; set; } // 含税金额

    }

}
