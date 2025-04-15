namespace CloudTrade.Domain
{
    public class BaseEntity
    {
        [SugarColumn(ColumnName = "Id", IsPrimaryKey = true, IsNullable = false, ColumnDataType = "uniqueidentifier", ColumnDescription ="编号")]
        public Guid Id { get; set; }
        [SugarColumn(ColumnName = "CreateTime", IsNullable = false, ColumnDataType = "varchar", Length = 100, ColumnDescription = "创建时间")]
        public string CreateTime { get; set; }
        [SugarColumn(ColumnName = "LastUpdateTime", IsNullable = false, ColumnDataType = "varchar", Length = 100, ColumnDescription = "最后修改时间")]
        public string LastUpdateTime { get; set; }
    }
}
