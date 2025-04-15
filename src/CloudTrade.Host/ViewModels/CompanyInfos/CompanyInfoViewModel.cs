using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.CompanyInfos
{
    public class CompanyInfoViewModel : BindableBase, INavigationAware
    {
        private readonly IBaseService db;
        public CompanyInfoViewModel(IBaseService db)
        {
            this.db = db;
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
           
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }
    }
}
