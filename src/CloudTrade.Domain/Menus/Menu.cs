using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.Menus
{
    [SugarTable(TableName = "Menu", TableDescription = "菜单表")]
    public class Menu:BaseEntity
    {
        [SugarColumn(ColumnName = "MenuName", IsNullable = false, ColumnDataType = "varchar", Length = 100, ColumnDescription = "MenuName")]
        public string MenuName { get; set; }
        [SugarColumn(ColumnName = "DisplayName", IsNullable = false, ColumnDataType = "varchar", Length = 100, ColumnDescription = "DispalyName")]
        public string DisplayName { get; set; }
        [SugarColumn(ColumnName = "ParentId", IsNullable = true, ColumnDataType = "uniqueidentifier", ColumnDescription = "父id")]
        public Guid ParentId { get; set; }

        [SugarColumn(ColumnName = "Icon", IsNullable = false, ColumnDataType = "nvarchar", Length = 100, ColumnDescription = "图标")]
        public string Icon { get; set; }
        [SugarColumn(ColumnName = "Sort", IsNullable = false, ColumnDataType = "int", ColumnDescription = "排序")]
        public int Sort { get; set; }
        [SugarColumn(ColumnName = "TargetView", IsNullable = false, ColumnDataType = "varchar", Length = 255, ColumnDescription = "视图名称")]
        public string TargetView { get; set; }
    }
}
