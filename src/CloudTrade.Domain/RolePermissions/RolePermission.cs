using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.RolePermissions
{
    [SugarTable(TableName = "RolePermission", TableDescription = "角色权限")]

    public class RolePermission
    {
        [SugarColumn(ColumnName = "RoleId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "角色编号")]
        public Guid RoleId { get; set; }
        [SugarColumn(ColumnName = "PermissionId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "权限编号")]
        public Guid PermissionId { get; set; }

    }
}
