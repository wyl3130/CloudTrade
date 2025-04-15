using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.CustomerCompanys
{
    [SugarTable(TableName = "CustomerCompany", TableDescription = "客户公司表")]
    public class CustomerCompany:BaseEntity
    {
        [SugarColumn(ColumnName = "CustomerCategoryId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "客户分类编号")]
        public Guid CustomerCategoryId { get; set; }
        [SugarColumn(ColumnName = "CustomerCompanyName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "客户公司名称")]
        public string CustomerCompanyName { get; set; }
        [SugarColumn(ColumnName = "ContactName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "联系人")]
        public string ContactName { get; set; }
        [SugarColumn(ColumnName = "Phone", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "手机")]
        public string Phone { get; set; }
        //[SugarColumn(ColumnName = "MobilePhone", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "电话")]
        //public string MobilePhone { get; set; }
        [SugarColumn(ColumnName = "Address", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "地址")]
        public string Address { get; set; }
        [SugarColumn(ColumnName = "PostalCode", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "邮编")]
        public string PostalCode { get; set; }
        [SugarColumn(ColumnName = "Email", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "邮箱")]
        public string Email { get; set; }
        [SugarColumn(ColumnName = "Fax", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "传真")]
        public string Fax { get; set; }
        [SugarColumn(ColumnName = "BankName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "开户行")]
        public string BankName { get; set; }
        [SugarColumn(ColumnName = "BankCardNo", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "银行卡账户")]
        public string BankCardNo { get; set; }
        [SugarColumn(ColumnName = "LegalRepresentative", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "法人")]
        public string LegalRepresentative { get; set; }
        //[SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        //public string Remark { get; set; }
    }
}
