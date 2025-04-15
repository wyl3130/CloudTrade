using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.Resources.Helper
{
    public static class PageHelper
    {
        public static int PageSize(int index)
        {
            switch (index)
            {
                case 0:
                    return 10;

                case 1:
                    return 20;

                case 2:
                    return 50;


                default:
                    return 10;

            }
        }
    }
}
