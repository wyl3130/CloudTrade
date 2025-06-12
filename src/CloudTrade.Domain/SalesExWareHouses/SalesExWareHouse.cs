using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.SalesExWareHouses
{
    [SugarTable("SalesExWareHouse", TableDescription = "销售出库单据")]
    public class SalesExWareHouse : BaseEntity
    {

        [SugarColumn(ColumnName = "ExWareHouseDate", IsNullable = false, ColumnDataType = "nvarchar", Length = 100, ColumnDescription = "出库时间")]
        public string ExWareHouseDate { get; set; } // 出库时间

        [SugarColumn(ColumnName = "CustomerCompanyId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的客户Id")]
        public Guid CustomerCompanyId { get; set; } // 关联的客户Id

        [SugarColumn(ColumnName = "WareHouseId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的仓库Id")]
        public Guid WareHouseId { get; set; } // 关联的仓库Id

        [SugarColumn(ColumnName = "PaymentCategoryId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的支付方式id")]
        public Guid PaymentCategoryId { get; set; } // 关联的支付方式id

        [SugarColumn(ColumnName = "PaymentAccountId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的支付账户id")]
        public Guid PaymentAccountId { get; set; } // 关联的支付账户id

        [SugarColumn(ColumnName = "CurrentAmount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "付款金额")]
        public double CurrentAmount { get; set; } // 付款金额

        [SugarColumn(ColumnName = "RealName", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "经办人")]
        public string RealName { get; set; } // 经办人

        [SugarColumn(ColumnName = "Preparer", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "制单人")]
        public string Preparer { get; set; } // 制单人

        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "备注")]
        public string Remark { get; set; } // 备注

        [SugarColumn(ColumnName = "SalesModeId", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "销售方式")]
        public Guid SalesModeId { get; set; } // 销售方式

        [SugarColumn(ColumnName = "OrderState", IsNullable = false, ColumnDataType = "int", ColumnDescription = "订单状态 0未审核 1已审核")]
        public int OrderState { get; set; } // 订单状态 0未审核 1已审核

        [SugarColumn(ColumnName = "SalesOrderId", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "关联的销售单据id")]
        public string SalesOrderId { get; set; } // 关联的销售单据id

        [SugarColumn(ColumnName = "CodeNo", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "常规编码 例：XS-2022-11-17-001")]
        public string CodeNo { get; set; } // 常规编码 例：XS-2022-11-17-001
    }
}
