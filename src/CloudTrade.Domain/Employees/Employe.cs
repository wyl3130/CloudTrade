using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.Employees
{
    [SugarTable(TableName = "Employe", TableDescription = "员工表")]

    public class Employe:BaseEntity
    {
        [SugarColumn(ColumnName = "RealName", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "真实姓名")]
        public string RealName { get; set; }
        [SugarColumn(ColumnName = "Sex", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "性别")]
        public string Sex { get; set; }
        [SugarColumn(ColumnName = "BornDate", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "出生年月")]
        public string BornDate { get; set; }
        [SugarColumn(ColumnName = "Education", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "学历")]
        public string Education { get; set; }
        [SugarColumn(ColumnName = "DepartmentId", IsNullable = false, ColumnDataType = "uniqueidentifier",ColumnDescription = "部门")]
        public Guid DepartmentId { get; set; }
        [SugarColumn(ColumnName = "PositionId", IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription = "职位编号")]
        public Guid PositionId { get; set; }    
        [SugarColumn(ColumnName = "Salary", IsNullable = false, ColumnDataType = "float", ColumnDescription = "薪资")]
        public double Salary { get; set; }
        [SugarColumn(ColumnName = "Phone", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "手机")]
        public string Phone { get; set; }

        [SugarColumn(ColumnName = "Address", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "地址")]
        public string Address { get; set; }
        [SugarColumn(ColumnName = "Email", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "邮箱")]
        public string Email { get; set; }
        [SugarColumn(ColumnName = "PostalCode", IsNullable = false, ColumnDataType = "varchar", Length = 50, ColumnDescription = "邮编")]
        public string PostalCode { get; set; }
        [SugarColumn(ColumnName = "Remark", IsNullable = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
