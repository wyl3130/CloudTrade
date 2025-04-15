using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.SalesModes
{
    [SugarTable(TableName = "SalesMode", TableDescription = "销售方式表")]
    public class SalesMode:BaseEntity
    {
        [SugarColumn(ColumnName = "ModeName", IsNullable = false, ColumnDataType = "varchar",Length =50, ColumnDescription = "方式名称")]
        public string ModeName { get; set; }
        [SugarColumn(ColumnName = "Sort", IsNullable = false, ColumnDataType = "int", ColumnDescription = "排序")]
        public int Sort { get; set; }
    }
}
