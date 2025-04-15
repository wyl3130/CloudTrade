using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.CustomerCategorys
{
    [SugarTable(TableName = "CustomerCategory", TableDescription = "客户分类表")]

    public class CustomerCategory:BaseEntity
    {
        [SugarColumn(ColumnName = "CustomerCategoryName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "客户分类名称")]
        public string CustomerCategoryName { get; set; }
        //[SugarColumn(ColumnName = "PId" ,IsNullable = true, ColumnDataType = "uniqueidentifier", ColumnDescription = "PId")]
        //public Guid PId { get; set; }
        [SugarColumn(ColumnName = "Sort", IsNullable = false, ColumnDataType = "int", ColumnDescription = "排序")]
        public int Sort { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
