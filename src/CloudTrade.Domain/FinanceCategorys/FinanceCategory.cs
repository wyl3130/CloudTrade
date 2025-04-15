using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.FinanceCategorys
{
    [SugarTable(TableName = "FinanceCategory", TableDescription = "其他收入支出")]
    public class FinanceCategory:BaseEntity
    {
        [SugarColumn(ColumnName = "FinanceCategoryName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "项目名称")]
        public string FinanceCategoryName { get; set; }
        [SugarColumn(ColumnName = "FinanceCategoryType", IsNullable = false, ColumnDataType = "int", ColumnDescription = "0 收入 1支出")]
        public int FinanceCategoryType { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
