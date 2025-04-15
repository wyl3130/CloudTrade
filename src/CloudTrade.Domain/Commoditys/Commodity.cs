using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.Commoditys
{
    [SugarTable(TableName ="Commodity",TableDescription ="商品表")]
    public class Commodity:BaseEntity
    {
        [SugarColumn(ColumnName ="CommodityName",ColumnDataType ="varchar",Length =255,ColumnDescription ="商品名称",IsNullable =false)]
        public string CommodityName { get; set; }
        [SugarColumn(ColumnName = "CommodityCategoryId", ColumnDataType = "uniqueidentifier ", ColumnDescription = "商品类别Id", IsNullable = false)]
        public Guid CommodityCategoryId { get; set; }
        [SugarColumn(ColumnName = "CommodityCompanyId", ColumnDataType = "uniqueidentifier ", ColumnDescription = "商品分类Id", IsNullable = false)]
        public Guid CommodityCompanyId { get; set; }
        [SugarColumn(ColumnName = "MaxStockCount", ColumnDataType = "int", ColumnDescription = "库存上限", IsNullable = false)]
        public int MaxStockCount { get; set; }
        [SugarColumn(ColumnName = "MinStockCount", ColumnDataType = "int", ColumnDescription = "库存下限", IsNullable = false)]
        public int MinStockCount { get; set; }
        [SugarColumn(ColumnName = "BarCode", ColumnDataType = "varchar", Length = 255, ColumnDescription = "条形码", IsNullable = false)]
        public string BarCode { get; set; }
        [SugarColumn(ColumnName = "PurchasePrice", ColumnDataType = "float", ColumnDescription = "进价", IsNullable = false)]
        public double PurchasePrice { get; set; }
        [SugarColumn(ColumnName = "Price", ColumnDataType = "float", ColumnDescription = "售价", IsNullable = false)]
        public double Price { get; set; }
        [SugarColumn(ColumnName = "Remark", ColumnDataType = "varchar", Length = 255, ColumnDescription = "备注", IsNullable = true)]
        public string Remark { get; set; }
        [SugarColumn(ColumnName = "Ppecification", ColumnDataType = "varchar", Length = 255, ColumnDescription = "规格", IsNullable = false)]
        public string Ppecification { get; set; }
        [SugarColumn(ColumnName = "Sort", ColumnDataType = "int", ColumnDescription = "排序", IsNullable = false)]
        public int Sort { get; set; }
        [SugarColumn(ColumnName = "CodeNo", ColumnDataType = "varchar", Length = 255, ColumnDescription = "商品编码 YZ-001", IsNullable = false)]
        public string CodeNo { get; set; }

    }
}
