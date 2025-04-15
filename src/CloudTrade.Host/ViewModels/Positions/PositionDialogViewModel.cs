using CloudTrade.Domain.Departments;
using CloudTrade.Domain.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Positions
{
    public class PositionDialogViewModel : BindableBase
    {
        private readonly IBaseService db;
        public PositionDialogViewModel(IBaseService db)
        {
            this.db = db;
        }
        private string title = "添加";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private Position entity = new() { LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
        public Position Entity
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
                        if (Entity != null && !string.IsNullOrEmpty(Entity.PositionName))
                        {
                            var list = db.Queryable<Position>().Where(x => x.PositionName.Equals(Entity.PositionName));
                            if (list.Count() != 0)
                            {
                                HandyControl.Controls.MessageBox.Show("职位已存在", "添加", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.InsertAsync<Position>(Entity);
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
                        if (Entity != null && !string.IsNullOrEmpty(Entity.PositionName))
                        {
                            //var list = db.Queryable<Position>().Where(x => x.PositionName.Equals(Entity.PositionName));
                            //if (list.Count() != 0)
                            //{
                            //    HandyControl.Controls.MessageBox.Show("职位已存在", "修改", MessageBoxButton.OK, MessageBoxImage.Error);
                            //    return;
                            //}
                            var result = await db.UpdateAsync<Position>(Entity);
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

        private ObservableCollection<Department> departmentList;
        public ObservableCollection<Department> DepartmentList
        {
            get => departmentList;
            set => SetProperty(ref departmentList, value);
        }
    }
}