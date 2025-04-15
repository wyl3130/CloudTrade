using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.Departments
{
    [SugarTable(TableName = "Department", TableDescription = "部门表")]
    public class Department:BaseEntity
    {
        [SugarColumn(ColumnName = "DepartmentName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "部门名称")]
        public string DepartmentName { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
