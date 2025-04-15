
using HandyControl.Controls;

namespace CloudTrade.Host.ViewModels.CommodityCategorys
{
    public class CommodityCategoryDialogViewModel : BindableBase
    {
        private readonly ICommodityCategoryService db;
        public CommodityCategoryDialogViewModel(ICommodityCategoryService db)
        {
            this.db = db;
        }

        private string title = "添加";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private CommodityCategory entity = new CommodityCategory() ;
        public CommodityCategory Entity
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
                    if (arg is System.Windows.Window view)
                    {
                        if (Title.Equals("添加"))
                        {
                            if (Entity != null && !string.IsNullOrEmpty(Entity.CommodityCategoryName))
                            {
                                Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                var result = await db.InsertAsync<CommodityCategory>(Entity);
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
                            if (Entity != null && !string.IsNullOrEmpty(Entity.CommodityCategoryName))
                            {
                                var result = await db.UpdateAsync<CommodityCategory>(Entity);
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
                catch (Exception ex)
                {
                    Growl.Error(ex.Message);
                }
            });
        }

        public DelegateCommand<object> closeCommand
        {
            get
            {
                return new DelegateCommand<object>((arg) =>
                {
                    if (arg is System.Windows.Window view)
                    {
                        view.Close();
                    }

                });
            }
        }
    }
}
