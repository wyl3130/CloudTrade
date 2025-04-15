using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.OtherWareHouses
{
    [SugarTable("OtherWareHouse", TableDescription = "其他入库表")]
    public class OtherWareHouse : BaseEntity
    {

        [SugarColumn(ColumnName = "WareHouseId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的入库仓Id")]
        public Guid WareHouseId { get; set; } // 关联的入库仓Id

        [SugarColumn(ColumnName = "DepartmentId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的部门Id")]
        public Guid DepartmentId { get; set; } // 关联的部门Id

        [SugarColumn(ColumnName = "RealName", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "经办人")]
        public string RealName { get; set; } // 经办人

        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "备注")]
        public string Remark { get; set; } // 备注

        [SugarColumn(ColumnName = "Preparer", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "制单人")]
        public string Preparer { get; set; } // 制单人

        [SugarColumn(ColumnName = "WareHouseMode", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "类型：赠送、借出、委托销售")]
        public string WareHouseMode { get; set; } // 类型：赠送、借出、委托销售

        [SugarColumn(ColumnName = "WarehouseDate", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "入库时间")]
        public string WarehouseDate { get; set; } // 入库时间

        [SugarColumn(ColumnName = "CodeNo", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "常规编码")]
        public string CodeNo { get; set; } // 常规编码
    }

}
