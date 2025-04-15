using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.PaymentCategorys
{
    [SugarTable(TableName = "PaymentCategory", TableDescription = "支付方式表")]
    public class PaymentCategory:BaseEntity
    {
        [SugarColumn(ColumnName = "PaymentCategoryName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "支付方式名称")]
        public string PaymentCategoryName { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
