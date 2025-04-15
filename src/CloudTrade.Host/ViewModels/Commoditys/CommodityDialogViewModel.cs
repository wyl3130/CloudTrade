using CloudTrade.Domain.Commoditys;


namespace CloudTrade.Host.ViewModels.Commoditys
{
    public class CommodityDialogViewModel:BindableBase
    {
        private readonly ICommodityService db;
        public CommodityDialogViewModel(ICommodityService db)
        {
            this.db = db;
        }
        private string title="添加";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private Commodity entity = new() {  };
        public Commodity Entity
        {
            get => entity;
            set => SetProperty(ref entity, value);
        }
        public DelegateCommand<object> enterCommand
        {
            get => new DelegateCommand<object>(async (arg) =>
            {
                try
                {
                    if (arg is Window view)
                    {
                        if (Title.Equals("添加"))
                        {
                            if (Entity != null && !string.IsNullOrEmpty(Entity.CommodityName))
                            {
                                Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                var result = await db.InsertAsync<Commodity>(Entity);
                                if (result)
                                {

                                    view.DialogResult = true;
                                    view.Close();
                                }
                            }
                            else
                            {
                                HandyControl.Controls.MessageBox.Show("必填项为空", "添加", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            if (Entity != null && !string.IsNullOrEmpty(Entity.CommodityName))
                            {
                                Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                var result = await db.UpdateAsync<Commodity>(Entity);
                                if (result)
                                {

                                    view.DialogResult = true;
                                    view.Close();
                                }
                            }
                            else
                            {
                                HandyControl.Controls.MessageBox.Show("必填项为空", "添加", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    HandyControl.Controls.Growl.Error(ex.Message);
                }
            });
        }
        public DelegateCommand<object> closeCommand
        {
            get
            {
                return new DelegateCommand<object>((arg) =>
                {
                    if (arg is Window view)
                    {
                        view.Close();
                    }

                });
            }
        }
        private ObservableCollection<CommodityCategory> commodityCategoryList;
        public ObservableCollection<CommodityCategory> CommodityCategoryList
        {
            get => commodityCategoryList;
            set => SetProperty(ref commodityCategoryList, value);
        }
        private ObservableCollection<CommodityCompany> commodityCompanyList;
        public ObservableCollection<CommodityCompany> CommodityCompanyList
        {
            get => commodityCompanyList;
            set => SetProperty(ref commodityCompanyList, value);
        }
    }
}
