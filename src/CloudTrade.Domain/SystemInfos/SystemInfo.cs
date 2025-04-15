using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.SystemInfos
{
    [SugarTable(TableName = "SystemInfo", TableDescription = "系统详情表")]
    public class SystemInfo : BaseEntity
    {
        [SugarColumn(ColumnName = "Author", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "开发团队")]
        public string Author { get; set; }
        [SugarColumn(ColumnName = "Phone", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "联系电话")]
        public string Phone { get; set; }
        [SugarColumn(ColumnName = "Copying", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "版权信息")]
        public string Copying { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
