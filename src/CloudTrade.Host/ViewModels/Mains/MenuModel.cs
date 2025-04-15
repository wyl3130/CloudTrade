using CloudTrade.Domain.Shared.Prisms;
using CloudTrade.Host.Resources.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Mains
{
    public class MenuModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private IRegionNavigationJournal journal;
        private readonly IEventAggregator eventAggregator;
        public MenuModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<ForwordChangeEvent>().Subscribe(forwordChange);
            eventAggregator.GetEvent<BackChangeEvent>().Subscribe(backChange);
        }
        public Guid Id { get; set; }
        public string MenuName { get; set; }
        public Guid ParentId { get; set; }
        public string Icon { get; set; }
        public int Sort { get; set; }
        public string TargetView { get; set; }
        public string DisplayName { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetProperty(ref _isExpanded, value); }
        }

        public ObservableCollection<MenuModel> Children { get; set; }

        public ICommand OpenViewCommand
        {
            get => new DelegateCommand(() =>
            {
                if ((Children == null || Children.Count == 0) &&
                    !string.IsNullOrEmpty(TargetView))
                {
                    // 在视图的生命周期中设置 KeepAlive
                    //regionManager.RequestNavigate("MainRegion", TargetView, new NavigationParameters { { "keepAlive", false } });
                    regionManager.RequestNavigate(regionName: RegionNames.MainRegion, TargetView, options => {
                       journal = options.Context.NavigationService.Journal;
                    });
                }
                else
                    IsExpanded = !IsExpanded;
            });
        }
        private void forwordChange()
        {
            //journal.GoForward();
            if (journal == null)
            {
                MessageBox.Show("空的");
            }
        }
        private void backChange()
        {
            journal.GoBack();
        }
    }
}
