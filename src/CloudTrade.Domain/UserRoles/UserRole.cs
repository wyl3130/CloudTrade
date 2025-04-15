using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.UserRoles
{
    [SugarTable(TableName = "UserRole", TableDescription = "用户角色")]
    public class UserRole:BaseEntity
    {
        [SugarColumn(ColumnName = "UserId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "用户编号")]
        public Guid UserId { get; set; }
        [SugarColumn(ColumnName = "RoleId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "角色编号")]
        public Guid RoleId { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
