using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.PaymentAccounts
{
    [SugarTable(TableName = "PaymentAccount", TableDescription = "支付账号")]

    public class PaymentAccount:BaseEntity
    {
        [SugarColumn(ColumnName = "PaymentAccountName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "支付账号")]
        public string PaymentAccountName { get; set; }
        [SugarColumn(ColumnName = "Holder", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "持有人")]
        public string Holder{ get; set; }
        [SugarColumn(ColumnName = "Amount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "金额")]
        public double Amount { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
