using CloudTrade.Domain.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Contracts.Menus
{
    public interface IMenuService:IBaseService
    {
        IEnumerable<Menu> SelectMenuList();
    }
}
