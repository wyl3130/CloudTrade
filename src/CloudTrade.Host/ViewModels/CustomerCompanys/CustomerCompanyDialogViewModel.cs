using CloudTrade.Application.Contracts.CustomerCompanys;
using CloudTrade.Domain.CustomerCategorys;
using CloudTrade.Domain.CustomerCompanys;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.CustomerCompanys
{
    public class CustomerCompanyDialogViewModel: BindableBase
    {
        private readonly ICustomerCompanyService db;
        public CustomerCompanyDialogViewModel(ICustomerCompanyService db)
        {
            this.db = db;
        }
        private string title = "添加";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private CustomerCompany entity = new() { };
        public CustomerCompany Entity
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
                            if (Entity != null && !string.IsNullOrEmpty(Entity.CustomerCompanyName))
                            {
                                Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                var result = await db.InsertAsync<CustomerCompany>(Entity);
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
                            if (Entity != null && !string.IsNullOrEmpty(Entity.CustomerCompanyName))
                            {
                                Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                var result = await db.UpdateAsync<CustomerCompany>(Entity);
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
