using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.ExWareHouseReceives
{
    [SugarTable("ExWareHouseReceive", TableDescription = "仓库领料（领用出库）")]
    public class ExWareHouseReceive : BaseEntity
    {

        [SugarColumn(ColumnName = "WareHouseId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的领料仓库Id")]
        public Guid WareHouseId { get; set; }  // 关联的领料仓库Id

        [SugarColumn(ColumnName = "DepartmentId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的领用部门Id")]
        public Guid DepartmentId { get; set; }  // 关联的领用部门Id

        [SugarColumn(ColumnName = "RealName", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "经办人")]
        public string RealName { get; set; }  // 经办人

        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "备注")]
        public string Remark { get; set; }  // 备注

        [SugarColumn(ColumnName = "Preparer", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "制单人")]
        public string Preparer { get; set; }  // 制单人

        [SugarColumn(ColumnName = "OrderState", IsNullable = false, ColumnDataType = "int", ColumnDescription = "订单状态 0未审核 1已审核 2已退库")]
        public int OrderState { get; set; }  // 订单状态 0未审核 1已审核 2已退库

        [SugarColumn(ColumnName = "ExWareHouseDate", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "出库时间")]
        public string ExWareHouseDate { get; set; }  // 出库时间

        [SugarColumn(ColumnName = "CodeNo", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "常规编码")]
        public string CodeNo { get; set; }  // 常规编码 例：xx-001
    }

}
