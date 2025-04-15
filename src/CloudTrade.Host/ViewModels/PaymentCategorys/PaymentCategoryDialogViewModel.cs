using CloudTrade.Application.Contracts.PaymentCategorys;
using CloudTrade.Domain.PaymentCategorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.PaymentCategorys
{
    public class PaymentCategoryDialogViewModel : BindableBase
    {
        private readonly IPaymentCategoryService db;
        public PaymentCategoryDialogViewModel(IPaymentCategoryService db)
        {
            this.db = db;
        }
        private string title = "添加";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private PaymentCategory entity = new PaymentCategory();
        public PaymentCategory Entity
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
                        if (Entity != null && !string.IsNullOrEmpty(Entity.PaymentCategoryName))
                        {
                            //var list = db.Queryable<PaymentCategory>().Where(x => x.PaymentCategoryName.Equals(Entity.PaymentCategoryName));
                            //if (list.Count() != 0)
                            //{
                            //    HandyControl.Controls.MessageBox.Show("商品单位已存在", "添加", MessageBoxButton.OK, MessageBoxImage.Error);
                            //    return;
                            //}
                            Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.InsertAsync<PaymentCategory>(Entity);
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
                        if (Entity != null && !string.IsNullOrEmpty(Entity.PaymentCategoryName))
                        {
                            //var list = db.Queryable<PaymentCategory>().Where(x => x.PaymentCategoryName.Equals(Entity.PaymentCategoryName));
                            //if (list.Count() != 0)
                            //{
                            //    HandyControl.Controls.MessageBox.Show("商品单位已存在", "修改", MessageBoxButton.OK, MessageBoxImage.Error);
                            //    return;
                            //}
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.UpdateAsync<PaymentCategory>(Entity);
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
