using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.PurchaseRefunds
{
    [SugarTable(TableName = "PurchaseRefund", TableDescription = "采购退货表")]
    public class PurchaseRefund:BaseEntity
    {
        [SugarColumn(ColumnName = "CustomerCompanyId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "供应商编号")]
        public Guid CustomerCompanyId { get; set; }
        [SugarColumn(ColumnName = "CurrentAmount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "金额")]
        public double CurrentAmount { get; set; }
        [SugarColumn(ColumnName = "PaymentCategoryId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "支付类型编号")]
        public Guid PaymentCategoryId { get; set; }
        [SugarColumn(ColumnName = "PaymentAccountId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "支付账户编号")]
        public Guid PaymentAccountId { get; set; }
        [SugarColumn(ColumnName = "RealName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "经办人")]
        public string RealName { get; set; }
        [SugarColumn(ColumnName = "WareHouseId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "仓库编号")]
        public Guid WareHouseId { get; set; }
        [SugarColumn(ColumnName = "Preparer", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "制单人")]
        public string Preparer { get; set; }
        [SugarColumn(ColumnName = "OrderState", IsNullable = false, ColumnDataType = "int", ColumnDescription = "状态 0 未审核 1审核")]
        public int OrderState { get; set; }
        [SugarColumn(ColumnName = "PurchaseWareHouseId", IsNullable = true, ColumnDataType = "uniqueidentifier", ColumnDescription = "出库订单编号")]
        public Guid PurchaseWareHouseId { get; set; }
        [SugarColumn(ColumnName = "ExWareHouseDate", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "出库时间")]
        public string ExWareHouseDate { get; set; }
        [SugarColumn(ColumnName = "CodeNo", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "编码")]
        public string CodeNo { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }


}
