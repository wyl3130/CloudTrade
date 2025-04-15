using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.PurchaseWareHouse
{
    [SugarTable(TableName = "PurchaseWareHouse", TableDescription = "采购入库")]
    public class PurchaseWareHouse : BaseEntity
    {
        [SugarColumn(IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnName = "CustomerCompanyId", ColumnDescription = "关联的供应商id")]
        public Guid CustomerCompanyId { get; set; } // 关联的供应商id

        [SugarColumn(IsNullable = false, ColumnDataType = "float", ColumnName = "CurrentAmount", ColumnDescription = "收款金额")]
        public float CurrentAmount { get; set; } // 收款金额


        [SugarColumn(IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnName = "WareHouseId", ColumnDescription = "关联的进货仓库Id")]
        public Guid WareHouseId { get; set; } // 关联的进货仓库Id
        [SugarColumn(IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnName = "PaymentCategoryId", ColumnDescription = "关联的支付方式id")]
        public Guid PaymentCategoryId { get; set; } // 关联的支付方式id
        [SugarColumn(IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnName = "PaymentAccountId", ColumnDescription = "关联的支付账户id")]
        public Guid PaymentAccountId { get; set; } // 关联的支付账户id

        [SugarColumn(IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnName = "RealName", ColumnDescription = "经办人")]
        public string RealName { get; set; } // 经办人
        [SugarColumn(IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnName = "Preparer", ColumnDescription = "制单人")]
        public string Preparer { get; set; } // 制单人
        [SugarColumn(IsNullable = false, ColumnDataType = "int", ColumnName = "OrderState", ColumnDescription = "订单状态 0未审核 1已审核 2已退货")]
        public int OrderState { get; set; } // 订单状态 0未审核 1已审核 2已退货
        [SugarColumn(IsNullable = true, ColumnDataType = "uniqueidentifier", Length = 255, ColumnName = "PurchaseOrderId", ColumnDescription = "关联的采购单据Id")]
        public Guid PurchaseOrderId { get; set; } // 关联的采购单据Id
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnName = "Remark", ColumnDescription = "备注")]
        public string Remark { get; set; } // 备注
        [SugarColumn(IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnName = "CodeNo", ColumnDescription = "常规编码 例：CG-2022-11-17-001")]
        public string CodeNo { get; set; } // 常规编码 例：CG-2022-11-17-001
        [SugarColumn(IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnName = "WareHouseDate", ColumnDescription = "入库日期")]
        public string WareHouseDate { get; set; } // 入库日期
    }

}
