using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.CommodityCategorys
{
    [SugarTable("CommodityCategory", TableDescription = "商品类型")]
    public class CommodityCategory: BaseEntity
    {
        [SugarColumn(ColumnName = "PId", IsNullable = true, ColumnDataType = "uniqueidentifier", ColumnDescription = "父编号")]
        public Guid PId { get; set; }
         
        [SugarColumn(ColumnName = "CommodityCategoryName ", ColumnDataType = "varchar", Length = 255, ColumnDescription = "商品类别名称", IsNullable = false)]
        public string CommodityCategoryName { get; set; }
        [SugarColumn(ColumnName = "Sort", ColumnDataType = "int", ColumnDescription = "排序", IsNullable = false)]
        public string Sort { get; set; }
        [SugarColumn(ColumnName = "Remark", ColumnDataType = "varchar", Length = 255, ColumnDescription = "备注", IsNullable = true)]
        public string Remark { get; set; }
    }
}
