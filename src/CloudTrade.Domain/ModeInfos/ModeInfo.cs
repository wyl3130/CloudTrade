using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.ModeInfos
{
    [SugarTable(TableName = "ModeInfo", TableDescription = "销售方式")]
    public class ModeInfo:BaseEntity
    {
        [SugarColumn(ColumnName = "ModeInfoName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "商品方式名称")]
        public string ModeInfoName { get; set; }
        [SugarColumn(ColumnName = "Sort", IsNullable = false, ColumnDataType = "int", ColumnDescription = "排序")]
        public int Sort { get; set; }
    }
}
