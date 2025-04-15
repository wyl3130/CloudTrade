using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.OtherFinances
{
    [SugarTable(TableName = "OtherFinance", TableDescription = "其他收入支出表")]
    public class OtherFinance:BaseEntity
    {
        [SugarColumn(ColumnName = "DWTimes", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "时间")]
        public string DWTimes { get; set; }
        [SugarColumn(ColumnName = "OtherFinanceType", IsNullable = false, ColumnDataType = "int", ColumnDescription = "类型")]
        public int OtherFinanceType { get; set; }
        [SugarColumn(ColumnName = "DepositAccountId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "转入账户编号")]
        public Guid DepositAccountId { get; set; }
        [SugarColumn(ColumnName = "BillNo", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "票据号码")]
        public string BillNo { get; set; }
        [SugarColumn(ColumnName = "Amount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "金额")]
        public double Amount { get; set; }
        [SugarColumn(ColumnName = "RealName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "经办人")]
        public string RealName { get; set; }
        [SugarColumn(ColumnName = "FinanceCategoryId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "支出收入项目编号")]
        public Guid FinanceCategoryId { get; set; }
        [SugarColumn(ColumnName = "PaymentCategoryId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "收付方式编号")]
        public Guid PaymentCategoryId { get; set; }
        [SugarColumn(ColumnName = "CodeNo", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "编码")]
        public string CodeNo { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
