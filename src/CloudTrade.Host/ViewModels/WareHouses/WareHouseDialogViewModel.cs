using CloudTrade.Domain.WareHouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.WareHouses
{
    public class WareHouseDialogViewModel:BindableBase
    {
        private readonly IBaseService db;
        public WareHouseDialogViewModel(IBaseService db)
        {
            this.db = db;
        }

        private string title = "添加";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private WareHouse entity = new() {  };
        public WareHouse Entity
        {
            get => entity;
            set => SetProperty(ref entity, value);
        }
        public DelegateCommand<object> enterCommand
        {
            get => new DelegateCommand<object>(async (arg) =>
            {
                if (arg is Window view)
                {
                    if (Title.Equals("添加"))
                    {
                        if (Entity != null && !string.IsNullOrEmpty(Entity.WareHouseName))
                        {
                            var list = db.Queryable<WareHouse>().Where(x => x.WareHouseName.Equals(Entity.WareHouseName));
                            if (list.Count() != 0)
                            {
                                HandyControl.Controls.MessageBox.Show("仓库已存在", "添加", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.InsertAsync<WareHouse>(Entity);
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
                        if (Entity != null && !string.IsNullOrEmpty(Entity.WareHouseName))
                        {

                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.UpdateAsync<WareHouse>(Entity);
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
    }
}
