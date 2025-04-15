using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.Payments
{
    [SugarTable("Payments", TableDescription = "付款单")]
    public class Payment : BaseEntity
    {
        [SugarColumn(ColumnName = "Id", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "编号")]
        public Guid Id { get; set; } // 编号

        [SugarColumn(ColumnName = "CreateTime", IsNullable = false, ColumnDataType = "nvarchar", Length = 100, ColumnDescription = "创建时间")]
        public string CreateTime { get; set; } // 创建时间

        [SugarColumn(ColumnName = "LastUpdateTime", IsNullable = false, ColumnDataType = "nvarchar", Length = 100, ColumnDescription = "最后修改时间")]
        public string LastUpdateTime { get; set; } // 最后修改时间

        [SugarColumn(ColumnName = "CustomerCompanyId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的客户/供应商编号")]
        public Guid CustomerCompanyId { get; set; } // 关联的客户/供应商编号

        [SugarColumn(ColumnName = "PaymentCategoryId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的收付方式编号")]
        public Guid PaymentCategoryId { get; set; } // 关联的收付方式编号

        [SugarColumn(ColumnName = "PaymentAccountId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "关联的收付账户编号")]
        public Guid PaymentAccountId { get; set; } // 关联的收付账户编号

        [SugarColumn(ColumnName = "RealName", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "经办人")]
        public string RealName { get; set; } // 经办人

        [SugarColumn(ColumnName = "Preparer", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "制单人")]
        public string Preparer { get; set; } // 制单人

        [SugarColumn(ColumnName = "CurrentAmount", IsNullable = false, ColumnDataType = "float", ColumnDescription = "本次付款")]
        public double CurrentAmount { get; set; } // 本次付款

        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "备注")]
        public string Remark { get; set; } // 备注

        [SugarColumn(ColumnName = "PaymentDate", IsNullable = false, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "付款日期")]
        public string PaymentDate { get; set; } // 付款日期

        [SugarColumn(ColumnName = "BillNo", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "单号")]
        public string BillNo { get; set; } // 单号

        [SugarColumn(ColumnName = "IsAdvance", IsNullable = false, ColumnDataType = "int", ColumnDescription = "是否为预付款")]
        public int IsAdvance { get; set; } // 是否为预付款 0否 1是

        [SugarColumn(ColumnName = "PaymentTypes", IsNullable = false, ColumnDataType = "int", ColumnDescription = "单据类型")]
        public int PaymentTypes { get; set; } // 单据类型 0销售退货 1采购入库

        [SugarColumn(ColumnName = "SalesRefundId", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "关联的销售退货单据编号")]
        public string SalesRefundId { get; set; } // 关联的销售退货单据编号

        [SugarColumn(ColumnName = "PurchaseWarehouseId", IsNullable = true, ColumnDataType = "nvarchar", Length = 255, ColumnDescription = "关联的采购入库单据编号")]
        public string PurchaseWarehouseId { get; set; } // 关联的采购入库单据编号

        [SugarColumn(ColumnName = "CodeNo", IsNullable = false, ColumnDataType = "nvarchar", Length = 255,  ColumnDescription = "常规编码")]
        public string CodeNo { get; set; } // 常规编码
    }

}
