using CloudTrade.Domain.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.Positions
{
    public class PositionDto:Position
    {
        public string DepartmentName { get; set; }
    }
}
