using CloudTrade.Application.Contracts.FinanceCategorys;
using CloudTrade.Domain.FinanceCategorys;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.FinanceCategorys
{
    public class FinanceCategoryDialogViewModel:BindableBase
    {
        private readonly IFinanceCategoryService db;
        public FinanceCategoryDialogViewModel(IFinanceCategoryService db)
        {
            this.db = db;
        }

        private string title = "添加";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private FinanceCategory entity = new ();
        public FinanceCategory Entity
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
                            if (Entity != null && !string.IsNullOrEmpty(Entity.FinanceCategoryName))
                            {
                                Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                var result = await db.InsertAsync<FinanceCategory>(Entity);
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
                            if (Entity != null && !string.IsNullOrEmpty(Entity.FinanceCategoryName))
                            {
                                Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                var result = await db.UpdateAsync<FinanceCategory>(Entity);
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
