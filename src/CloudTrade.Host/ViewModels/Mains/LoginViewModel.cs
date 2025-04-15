using CloudTrade.Domain.Users;
using CloudTrade.Host.Views.Mains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Mains
{
    public class LoginViewModel : BindableBase
    {
        private readonly IUserService db;
        private readonly IMenuService mdb;
        public LoginViewModel(IUserService db,IMenuService mdb)
        {
            this.db = db;
            this.mdb = mdb;

        }

        public DelegateCommand<object> LoginCommand
        {
            get
            {
                return new DelegateCommand<object>(async (arg) =>
                {
                    if (!UserName.Equals("") && !PassWord.Equals(""))
                    {
                        try
                        {
                            var result = await db.LoginAsync(UserName, PassWord);
                            if (result)
                            {
                                HandyControl.Controls.MessageBox.Show($"登录成功，欢迎：{UserName}");
                                var mainView = new MainWindowView();
                                System.Windows.Application.Current.MainWindow = mainView;
                                AppData.Menus = mdb.SelectMenuList();
                                if (arg is LoginView lview)
                                {
                                    mainView.Show();
                                    lview.Close();
                                }
                            }
                            else
                            {
                                HandyControl.Controls.MessageBox.Show("用户名或密码有误");
                            }
                        }
                        catch (Exception ex)
                        {
                            HandyControl.Controls.MessageBox.Show($"登录异常：{ex.Message}");
                        }
                    }
                    else
                    {
                        HandyControl.Controls.MessageBox.Show("用户名或密码为空");
                    }
                });
            }
        }
        private string userName = "";
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }
        private string passWord = "";
        public string PassWord
        {
            get { return passWord; }
            set { SetProperty(ref passWord, value); }
        }
    }
}
