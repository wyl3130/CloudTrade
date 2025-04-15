using CloudTrade.Application.Contracts.Employees;
using CloudTrade.Domain.Departments;
using CloudTrade.Domain.Employees;
using CloudTrade.Domain.Positions;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Employees
{
    public class EmployeDialogViewModel:BindableBase
    {
        private readonly IEmployeService db;
        public EmployeDialogViewModel(IEmployeService db)
        {
            this.db = db;
        }
        private string title = "添加";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private Employe entity = new Employe();
        public Employe Entity
        {
            get => entity;
            set => SetProperty(ref entity, value);
        }
        private ObservableCollection<Department> departmentList;
        public ObservableCollection<Department> DepartmentList
        {
            get => departmentList;
            set => SetProperty(ref departmentList, value);
        }
        private ObservableCollection<Position> positionList;
        public ObservableCollection<Position> PositionList
        {
            get => positionList;
            set => SetProperty(ref positionList, value);
        }
        public DelegateCommand<object> enterCommand
        {
            get => new DelegateCommand<object>(async (arg) =>
            {
                if (arg is System.Windows.Window view)
                {
                    if (Title.Equals("添加"))
                    {
                        if (Entity != null && !string.IsNullOrEmpty(Entity.RealName))
                        {
                            var list = db.Queryable<Employe>().Where(x => x.RealName.Equals(Entity.RealName));
                            if (list.Count() != 0)
                            {
                                HandyControl.Controls.MessageBox.Show("员工已存在", "添加", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            Entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.InsertAsync<Employe>(Entity);
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
                        if (Entity != null && !string.IsNullOrEmpty(Entity.RealName))
                        {
                            //var list = db.Queryable<Employe>().Where(x => x.RealName.Equals(Entity.RealName));
                            //if (list.Count() != 0)
                            //{
                            //    HandyControl.Controls.MessageBox.Show("商品单位已存在", "修改", MessageBoxButton.OK, MessageBoxImage.Error);
                            //    return;
                            //}
                            Entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            var result = await db.UpdateAsync<Employe>(Entity);
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
        public DelegateCommand<object> cboCommand
        {
            get
            {
                return new DelegateCommand<object>(async (arg) =>
                {
                    try
                    {

                        if (arg is List<object> list)
                        {
                            // 尝试将 list[0] 转换为 Guid
                            if (list[0] != null && Guid.TryParse(list[0].ToString(), out Guid guidValue) && !string.IsNullOrEmpty(list[0].ToString()))
                            {
                                var did = DepartmentList.Where(x => x.Id.Equals(list[0])).FirstOrDefault();
                                if (did != null)
                                {
                                    var plist = await db.QueryableAsync<Position>(x => x.DepartmentId.Equals(did.Id));
                                    PositionList = new ObservableCollection<Position>(plist);
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
