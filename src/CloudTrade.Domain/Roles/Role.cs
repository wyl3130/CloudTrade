using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.Roles
{
    [SugarTable(TableName = "Role", TableDescription = "角色表")]
    public class Role: BaseEntity
    {
        [SugarColumn(ColumnName = "RoleName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "角色名称")]
        public string RoleName { get; set; }
        [SugarColumn(ColumnName = "State", IsNullable = false, ColumnDataType = "int", ColumnDescription = "状态")]
        public int State { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
