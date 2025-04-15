using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.SystemInfos
{
    public class SystemInfoViewModel:BindableBase
    {
        public SystemInfoViewModel()
        {
            t1Command = new DelegateCommand(t1);
        }

        public DelegateCommand t1Command ;
        public void t1()
        {

        }
        public DelegateCommand t2Command
        {
            set { t2Command = new DelegateCommand(t1); }
        }

        public DelegateCommand t3Command
        {
            get
            {
                return new DelegateCommand(() =>
                {

                });
            }
        }
        public DelegateCommand t4Command
        {
            get => new DelegateCommand(() =>
            {

            });
        }
        public DelegateCommand t5Command => new DelegateCommand(() => { 
        
        
        });
    }
}
