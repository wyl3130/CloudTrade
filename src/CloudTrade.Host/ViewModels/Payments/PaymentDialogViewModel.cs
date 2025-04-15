using CloudTrade.Application.Contracts.FinanceCategorys;
using CloudTrade.Application.Contracts.Payments;
using CloudTrade.Domain.FinanceCategorys;
using CloudTrade.Domain.Payments;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Payments
{
    public class PaymentDialogViewModel : BindableBase
    {
        private readonly IPaymentService db;
        public PaymentDialogViewModel(IPaymentService db)
        {
            this.db = db;
        }

        private string title = "添加";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private Payment entity = new();
        public Payment Entity
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
                            Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.InsertAsync<Payment>(Entity);
                            if (result)
                            {

                                view.DialogResult = true;
                                view.Close();
                            }
                        }
                        else
                        {
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.UpdateAsync<Payment>(Entity);
                            if (result)
                            {

                                view.DialogResult = true;
                                view.Close();
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
