using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.WareHouseAllocations
{
    [SugarTable("WareHouseAllocation", TableDescription = "仓库调拨表")]
    public class WareHouseAllocation : BaseEntity
    {
        [SugarColumn(ColumnName = "WareHouseId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的入库仓库ID")]
        public Guid WareHouseId { get; set; } // 关联的入库仓库ID

        [SugarColumn(ColumnName = "ExWareHouseId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的调出仓库ID")]
        public Guid ExWareHouseId { get; set; } // 关联的调出仓库ID

        [SugarColumn(ColumnName = "RealName", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "经办人")]
        public string RealName { get; set; } // 经办人

        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "备注")]
        public string Remark { get; set; } // 备注

        [SugarColumn(ColumnName = "Preparer", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "制单人")]
        public string Preparer { get; set; } // 制单人

        [SugarColumn(ColumnName = "OrderState", IsNullable = false, ColumnDataType = "int", ColumnDescription = "订单状态")]
        public int OrderState { get; set; } // 订单状态 0待审核 1已审核

        [SugarColumn(ColumnName = "WareHouseDate", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "调拨时间")]
        public string WareHouseDate { get; set; } // 调拨时间

        [SugarColumn(ColumnName = "CodeNo", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "常规编码")]
        public string CodeNo { get; set; } // 常规编码 例：xx-001
    }

}
