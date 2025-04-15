using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.MenuPermissions
{
    [SugarTable(TableName = "MenuPermission", TableDescription = "菜单权限表")]

    public class MenuPermission:BaseEntity
    {
        [SugarColumn(ColumnName = "MenuId",  IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "菜单编号")]
        public Guid MenuId { get; set; }
        [SugarColumn(ColumnName = "PermissionId",  IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "权限编号")]
        public Guid PermissionId { get; set; }
    }
}
