using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.DepositWithdrawals
{
    [SugarTable(TableName = "DepositWithdrawal", TableDescription = "存取款")]
    public class DepositWithdrawal:BaseEntity
    {
        [SugarColumn(ColumnName = "DWTime", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "存取款时间")]
        public string DWTime { get; set; }
        [SugarColumn(ColumnName = "DepositWithdrawalType", IsNullable = false, ColumnDataType = "int", ColumnDescription = "类型 0存款 1取款")]
        public int DepositWithdrawalType { get; set; }
        [SugarColumn(ColumnName = "PaymentAccountId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "账户编号")]
        public Guid PaymentAccountId { get; set; }
        [SugarColumn(ColumnName = "DepositAccountId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "账户编号")]
        public Guid DepositAccountId { get; set; }
        [SugarColumn(ColumnName = "Amount", IsNullable = false, ColumnDataType = "float",  ColumnDescription = "金额")]
        public double Amount { get; set; }
        [SugarColumn(ColumnName = "RealName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "经办人")]
        public string RealName { get; set; }
        [SugarColumn(ColumnName = "BillNo", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "票据号码")]
        public string BillNo { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
