using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.CompanyInfos
{
    [SugarTable(TableName = "CompanyInfo", TableDescription = "公司详情表")]
    public class CompanyInfo:BaseEntity
    {
        [SugarColumn(ColumnName = "CompanyName", IsNullable = false, ColumnDataType = "varchar",Length =50, ColumnDescription = "公司名称")]
        public string CompanyName { get; set; }
        [SugarColumn(ColumnName = "Phone", IsNullable = false, ColumnDataType = "varchar",Length =11, ColumnDescription = "公司电话")]
        public string Phone { get; set; }
        [SugarColumn(ColumnName = "Address", IsNullable = false, ColumnDataType = "varchar",Length =50, ColumnDescription = "公司地址")]
        public string Address { get; set; }
        [SugarColumn(ColumnName = "Fax", IsNullable = false, ColumnDataType = "varchar",Length =50, ColumnDescription = "公司传真")]
        public string Fax { get; set; }
        [SugarColumn(ColumnName = "Email", IsNullable = false, ColumnDataType = "varchar",Length =50, ColumnDescription = "公司邮箱")]
        public string Email { get; set; }
        [SugarColumn(ColumnName = "PostalCode", IsNullable = false, ColumnDataType = "varchar",Length =50, ColumnDescription = "邮编")]
        public string PostalCode { get; set; }
        [SugarColumn(ColumnName = "LegalRepresentative", IsNullable = false, ColumnDataType = "varchar",Length =50, ColumnDescription = "法人")]
        public string LegalRepresentative { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = false, ColumnDataType = "varchar",Length =50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
