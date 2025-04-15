using CloudTrade.Application.Contracts.ModeInfos;
using CloudTrade.Domain.Departments;
using CloudTrade.Domain.ModeInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.ModeInfos
{
    public class ModeInfoDialogViewModel : BindableBase
    {
        private readonly IModeInfoService db;
        public ModeInfoDialogViewModel(IModeInfoService db)
        {
            this.db = db;
        }
        public DelegateCommand<object> enterCommand
        {
            get => new DelegateCommand<object>(async (arg) =>
            {
                if (arg is Window view)
                {
                    if (Title.Equals("添加"))
                    {
                        if (Entity != null && !string.IsNullOrEmpty(Entity.ModeInfoName))
                        {
                            var list = db.Queryable<ModeInfo>().Where(x => x.ModeInfoName.Equals(Entity.ModeInfoName));
                            if (list.Count() != 0)
                            {
                                HandyControl.Controls.MessageBox.Show("部门已存在", "添加", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.InsertAsync<ModeInfo>(Entity);
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
                        if (Entity != null && !string.IsNullOrEmpty(Entity.ModeInfoName))
                        {
                            //var list = db.Queryable<Department>().Where(x => x.DepartmentName.Equals(Entity.DepartmentName));
                            //if (list.Count() != 0)
                            //{
                            //    HandyControl.Controls.MessageBox.Show("部门已存在", "修改", MessageBoxButton.OK, MessageBoxImage.Error);
                            //    return;
                            //}
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.UpdateAsync<ModeInfo>(Entity);
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
        private string title = "添加";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private ModeInfo entity = new ModeInfo();
        public ModeInfo Entity
        {
            get => entity;
            set => SetProperty(ref entity, value);
        }
    }
}
