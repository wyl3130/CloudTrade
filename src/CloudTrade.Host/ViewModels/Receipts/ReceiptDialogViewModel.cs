using CloudTrade.Application.Contracts.FinanceCategorys;
using CloudTrade.Application.Contracts.Receipts;
using CloudTrade.Domain.FinanceCategorys;
using CloudTrade.Domain.Receipts;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Receipts
{
    public class ReceiptDialogViewModel : BindableBase
    {
        private readonly IReceiptService db;
        public ReceiptDialogViewModel(IReceiptService db)
        {
            this.db = db;
        }

        private string title = "添加";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private Receipt entity = new();
        public Receipt Entity
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
                            var result = await db.InsertAsync<Receipt>(Entity);
                            if (result)
                            {

                                view.DialogResult = true;
                                view.Close();
                            }
                        }
                        else
                        {
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.UpdateAsync<Receipt>(Entity);
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
