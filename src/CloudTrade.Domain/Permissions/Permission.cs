using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.Permissions
{
    [SugarTable(TableName = "Permission", TableDescription = "权限表")]
    public class Permission:BaseEntity
    {
        [SugarColumn(ColumnName = "PermissionName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "权限名称")]
        public string PermissionName { get; set; }
        [SugarColumn(ColumnName = "PermissionCode", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "权限代码")]
        public string PermissionCode { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }

    }
}
