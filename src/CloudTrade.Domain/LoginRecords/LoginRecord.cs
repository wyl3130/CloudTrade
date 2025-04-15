using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Domain.LoginRecords
{
    [SugarTable(TableName = "LoginRecord", TableDescription = "登录记录")]
    public class LoginRecord:BaseEntity
    {
        public Guid UserId { get; set; }
        public string LoginIp { get; set; }
        public string LoginCity { get; set; }
        public string State { get; set; }
    }
}
