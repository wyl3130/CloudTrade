using CloudTrade.Application.Contracts.CustomerCategorys;
using CloudTrade.Domain.CustomerCategorys;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.CustomerCategorys
{
    public class CustomerCategoryDialogViewModel:BindableBase
    {
        private readonly ICustomerCategoryService db;
        public CustomerCategoryDialogViewModel(ICustomerCategoryService db)
        {
            this.db = db;
            CustomerCategoryList = new ObservableCollection<CustomerCategory>(db.Queryable<CustomerCategory>());
        }
        private string title = "添加";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private CustomerCategory entity = new() {};
        public CustomerCategory Entity
        {
            get => entity;
            set => SetProperty(ref entity, value);
        }
        private ObservableCollection<CustomerCategory> customerCategoryList;
        public ObservableCollection<CustomerCategory> CustomerCategoryList
        {
            get => customerCategoryList;
            set => SetProperty(ref customerCategoryList, value);
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
                            if (Entity != null && !string.IsNullOrEmpty(Entity.CustomerCategoryName))
                            {

                                Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                var result = await db.InsertAsync<CustomerCategory>(Entity);
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
                            if (Entity != null && !string.IsNullOrEmpty(Entity.CustomerCategoryName))
                            {
                                Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                var result = await db.UpdateAsync<CustomerCategory>(Entity);
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
