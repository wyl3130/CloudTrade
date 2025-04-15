using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.WareHouses
{
    [SugarTable(TableName = "WareHouse", TableDescription = "仓库表")]
    public class WareHouse:BaseEntity
    {
        [SugarColumn(ColumnName = "WareHouseName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "仓库名称")]
        public string WareHouseName { get; set; }
        [SugarColumn(ColumnName = "Address", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "仓库地址")]
        public string Address { get; set; }
        [SugarColumn(ColumnName = "RealName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "仓库负责人")]
        public string RealName { get; set; }
        [SugarColumn(ColumnName = "Phone", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "负责人电话")]
        public string Phone { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
