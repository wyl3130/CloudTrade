using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.SalesOrders
{
    [SugarTable("SalesOrder", TableDescription = "销售订单单据")]
    public class SalesOrder : BaseEntity
    {

        [SugarColumn(ColumnName = "ExWarehouseDate", IsNullable = false, ColumnDataType = "nvarchar", Length = 100, ColumnDescription = "出库时间")]
        public string ExWarehouseDate { get; set; } // 出库时间

        [SugarColumn(ColumnName = "WareHouseId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的仓库Id")]
        public Guid WareHouseId { get; set; } // 关联的仓库Id

        [SugarColumn(ColumnName = "CustomerCompanyId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的客户Id")]
        public Guid CustomerCompanyId { get; set; } // 关联的客户Id

        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "备注")]
        public string Remark { get; set; } // 备注

        [SugarColumn(ColumnName = "OrderState", IsNullable = false, ColumnDataType = "int", ColumnDescription = "订单状态 0未审核 1已审核")]
        public int OrderState { get; set; } // 订单状态 0未审核 1已审核

        [SugarColumn(ColumnName = "Preparer", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "制单人")]
        public string Preparer { get; set; } // 制单人

        [SugarColumn(ColumnName = "RealName", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "经办人")]
        public string RealName { get; set; } // 经办人

        [SugarColumn(ColumnName = "SalesMode", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "销售方式")]
        public string SalesMode { get; set; } // 销售方式

        [SugarColumn(ColumnName = "CodeNo", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "常规编码 例：XD-2022-11-17-001")]
        public string CodeNo { get; set; } // 常规编码 例：XD-2022-11-17-001
    }

}
