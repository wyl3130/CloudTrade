using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.CommodityCompanys
{

    public class CommodityCompanyDialogViewModel : BindableBase
    {
        private readonly ICommodityCompanyService db;
        public CommodityCompanyDialogViewModel(ICommodityCompanyService db)
        {
            this.db = db;
        }

        

        private string title = "添加";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private CommodityCompany entity = new() {  };
        public CommodityCompany Entity
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
                            if (Entity != null && !string.IsNullOrEmpty(Entity.CommodityCompanyName))
                            {
                                //var list = db.Queryable<CommodityCompany>().Where(x => x.CommodityCompanyName.Equals(Entity.CommodityCompanyName));
                                //if (list.Count() != 0)
                                //{
                                //    HandyControl.Controls.MessageBox.Show("商品单位已存在", "添加", MessageBoxButton.OK, MessageBoxImage.Error);
                                //    return;
                                //}
                                Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                var result = await db.InsertAsync<CommodityCompany>(Entity);
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
                            if (Entity != null && !string.IsNullOrEmpty(Entity.CommodityCompanyName))
                            {
                                //var list = db.Queryable<CommodityCompany>().Where(x => x.CommodityCompanyName.Equals(Entity.CommodityCompanyName));
                                //if (list.Count() != 0)
                                //{
                                //    HandyControl.Controls.MessageBox.Show("商品单位已存在", "修改", MessageBoxButton.OK, MessageBoxImage.Error);
                                //    return;
                                //}
                                Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                var result = await db.UpdateAsync<CommodityCompany>(Entity);
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
