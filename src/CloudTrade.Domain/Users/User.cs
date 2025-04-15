namespace CloudTrade.Domain.Users
{
    [SugarTable(TableName ="User",TableDescription ="用户表")]
    public class User: BaseEntity
    {
        [SugarColumn(ColumnName = "UserName", IsNullable = false, ColumnDataType = "varchar", Length = 100, ColumnDescription = "账户")]
        public string UserName { get; set; }
        [SugarColumn(ColumnName = "PassWord", IsNullable = false, ColumnDataType = "varchar", Length = 100, ColumnDescription = "密码")]
        public string PassWord { get; set; }
        [SugarColumn(ColumnName = "State", IsNullable = false, ColumnDataType = "int", ColumnDescription = "状态0正常 1封禁")]
        public int State { get; set; }
    }
}
