using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.SalesRefunds
{
    [SugarTable("SalesRefund", TableDescription = "销售退货单")]
    public class SalesRefund : BaseEntity
    {
        [SugarColumn(ColumnName = "WareHouseDate", IsNullable = false, ColumnDataType = "nvarchar", Length = 100, ColumnDescription = "出库时间")]
        public string WareHouseDate { get; set; } // 出库时间

        [SugarColumn(ColumnName = "CustomerCompanyId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的客户Id")]
        public Guid CustomerCompanyId { get; set; } // 关联的客户Id

        [SugarColumn(ColumnName = "CurrentAmount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "付款金额")]
        public double CurrentAmount { get; set; } // 付款金额

        [SugarColumn(ColumnName = "PaymentCategoryId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的支付方式Id")]
        public Guid PaymentCategoryId { get; set; } // 关联的支付方式Id

        [SugarColumn(ColumnName = "PaymentAccountId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的支付账户Id")]
        public Guid PaymentAccountId { get; set; } // 关联的支付账户Id

        [SugarColumn(ColumnName = "RealName", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "经办人")]
        public string RealName { get; set; } // 经办人

        [SugarColumn(ColumnName = "WareHouseId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的仓库Id")]
        public Guid WareHouseId { get; set; } // 关联的仓库Id

        [SugarColumn(ColumnName = "Preparer", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "制单人")]
        public string Preparer { get; set; } // 制单人

        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "备注")]
        public string Remark { get; set; } // 备注

        [SugarColumn(ColumnName = "OrderState", IsNullable = false, ColumnDataType = "int", ColumnDescription = "订单状态")]
        public int OrderState { get; set; } // 订单状态 0未审核 1已审核

        [SugarColumn(ColumnName = "SalesExWareHouseId", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "关联的出库单据Id")]
        public Guid SalesExWareHouseId { get; set; } // 关联的出库单据Id

        [SugarColumn(ColumnName = "CodeNo", IsNullable = false, ColumnDataType = "nvarchar", Length = 255,  ColumnDescription = "常规编码")]
        public string CodeNo { get; set; } // 常规编码 例：XT-2022-11-17-001
    }

}
