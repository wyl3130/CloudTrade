using CloudTrade.Application.Contracts.Menus;
using CloudTrade.Domain.Shared.Enums;
using CloudTrade.Domain.Shared.Prisms;
using CloudTrade.Host.Resources.Event;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Mains
{
    public class TreeMenuViewModel : BindableBase
    {
        private readonly IMenuService db;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        private IEnumerable<Domain.Menus.Menu> menuList = null;
        public TreeMenuViewModel(IMenuService db, IRegionManager regionManager, IEventAggregator eventAggregator)
        {

            this.db = db;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            LanguageChange();
            eventAggregator.GetEvent<LanguageChangeEvent>().Subscribe(LanguageChange);
        }
        public void LanguageChange()
        {
            Menus = new ObservableCollection<MenuModel>();
            menuList = db.SelectMenuList();
            FillMenus(Menus, new Guid());
        }
        private void FillMenus(ObservableCollection<MenuModel> menus, Guid parentId)
        {

            var sub = menuList.Where(m => m.ParentId == parentId).OrderBy(o => o.Sort);

            if (sub.Count() > 0)
            {
                foreach (var item in sub)
                {
                    MenuModel mm = new MenuModel(regionManager, eventAggregator)
                    {
                        MenuName = item.MenuName,
                        Icon = item.Icon,
                        TargetView = item.TargetView,
                        DisplayName = (string)System.Windows.Application.Current.Resources[item.MenuName]
                        //    Growl.Info((string)System.Windows.Application.Current.Resources["DarkMode"]);
                    };
                    menus.Add(mm);

                    FillMenus(mm.Children = new ObservableCollection<MenuModel>(), item.Id);

                }
            }
        }
        private ObservableCollection<MenuModel> menus;
        public ObservableCollection<MenuModel> Menus
        {
            get { return menus; }
            set { SetProperty(ref menus, value); }
        }


    }



}
