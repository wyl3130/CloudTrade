using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.PurchaseOrders
{
    [SugarTable(TableName = "PurchaseOrder", TableDescription = "采购订单")]
    public class PurchaseOrder:BaseEntity
    {


        [SugarColumn(ColumnName = "CustomerCompanyId ", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "供应商编号")]
        public Guid CustomerCompanyId { get; set; }
        [SugarColumn(ColumnName = "WareHouseId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "仓库编号")]
        public Guid WareHouseId { get; set; }
        [SugarColumn(ColumnName = "RealName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "经办人")]
        public string RealName { get; set; }
        [SugarColumn(ColumnName = "OrderState", IsNullable = false, ColumnDataType = "int", ColumnDescription = "状态 0 审核 1 未审核")]
        public int OrderState { get; set; }
        [SugarColumn(ColumnName = "Preparer", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "制单人")]
        public string Preparer { get; set; }
        [SugarColumn(ColumnName = "WareHouseDate", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "入库时间")]
        public string WareHouseDate { get; set; }
        [SugarColumn(ColumnName = "CodeNo", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "编码")]
        public string CodeNo { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
