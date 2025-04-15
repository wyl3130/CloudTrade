using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.WareHouseStocktakings
{
    [SugarTable(TableName = "WareHouseStocktaking", TableDescription = "仓库盘点表")]
    public class WareHouseStocktaking:BaseEntity
    {
        [SugarColumn(ColumnName = "WareHouseId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "编号")]
        public Guid WareHouseId { get; set; }
        [SugarColumn(ColumnName = "RealName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "经办人")]
        public string RealName { get; set; }
        [SugarColumn(ColumnName = "Preparer", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "制单人")]
        public string Preparer { get; set; }
        [SugarColumn(ColumnName = "OrderState", IsNullable = false, ColumnDataType = "int", ColumnDescription = "状态")]
        public int OrderState { get; set; }
        [SugarColumn(ColumnName = "WareHouseStocktakingDate", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "盘点时间")]
        public string WareHouseStocktakingDate { get; set; }
        [SugarColumn(ColumnName = "CodeNo", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "编码")]
        public string CodeNo { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
