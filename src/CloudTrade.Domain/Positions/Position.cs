using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.Positions
{
    [SugarTable(TableName = "Position", TableDescription = "职位表")]
    public class Position:BaseEntity
    {
        [SugarColumn(ColumnName = "DepartmentId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "部门编号")]
        public Guid DepartmentId { get; set; }
        [SugarColumn(ColumnName = "PositionName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "职位名称")]
        public string PositionName { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
            