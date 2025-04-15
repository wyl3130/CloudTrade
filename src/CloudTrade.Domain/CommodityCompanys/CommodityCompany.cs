using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.CommodityCompanys
{
    [SugarTable("CommodityCompany", TableDescription = "商品单位")]
    public class CommodityCompany: BaseEntity
    {
        [SugarColumn(ColumnName = "CommodityCompanyName ", ColumnDataType = "varchar", Length = 255, ColumnDescription = "商品单位名称", IsNullable = false)]
        public string CommodityCompanyName { get; set; }
        [SugarColumn(ColumnName = "Remark", ColumnDataType = "varchar", Length = 255, ColumnDescription = "备注", IsNullable = true)]
        public string Remark { get; set; }
    }
}
