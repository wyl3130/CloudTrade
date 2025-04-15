using Microsoft.Toolkit.Uwp.Notifications;
using Prism.Navigation.Regions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Windows.UI.WebUI;

namespace Host.Test
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IRegionManager regionManager;
        IRegionNavigationJournal journal;
        public ObservableCollection<string> GenderList { get; set; }
        public MainWindowViewModel(IRegionManager regionManager)
        {
            GenderList = new ObservableCollection<string> { "男", "女" };
            this.regionManager = regionManager;

           // BinaryFormatter binaryFormatter = new();
        }
        public DelegateCommand btn1Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    // Requires Microsoft.Toolkit.Uwp.Notifications NuGet package version 7.0 or greater
                    new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddArgument("conversationId", 9813)
                        .AddText("第一行")
                        .AddText("第二行")
                        .AddInlineImage(new Uri("D:\\桌面\\CloudTrade云商\\CloudTrade\\src\\CloudTrade.Host\\Resources\\Images\\avatar.jpg"))
                        .AddAppLogoOverride(new Uri("D:\\桌面\\CloudTrade云商\\CloudTrade\\src\\CloudTrade.Host\\Resources\\Images\\avatar.jpg"), ToastGenericAppLogoCrop.Circle)
                        .AddInputTextBox("", placeHolderContent: "输入点东西吧", title: "这是标题")
                        .AddButton(
                            new ToastButton()
                            .SetContent("按钮1")
                            .AddArgument("action", "1")
                            .SetBackgroundActivation()
                        ).AddButton(
                            new ToastButton()
                            .SetContent("按钮2")
                            .AddArgument("action", "3")
                            .SetBackgroundActivation()
                        ).AddButton(
                            new ToastButton()
                            .SetContent("按钮3")
                            .AddArgument("action", "3")
                   .AddArgument("imageUrl")
                        )
                  .AddComboBox("comboBoxId", "选择一个选项", "Option1",
                 ("Option1", "选项1"),
                 ("Option2", "选项2"),
                 ("Option3", "选项3"))

                        .AddAudio(new Uri("D:\\Documents\\Overwatch\\videos\\overwatch\\file_25-02-24_19-47-34.mp4"))
                        .AddHeader("重要通知", "这是一个标题", "此为副标题")
                        //  .AddProgressBar("",100)

                        .Show(toast =>
                        {
                            toast.ExpirationTime = DateTime.Now.AddDays(2);
                        }); // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 6 (or later), then your TFM must be net6.0-windows10.0.17763.0 or greater




                });
            }
        }

        public DelegateCommand btn2Command
        {
            get => new DelegateCommand(() =>
            {
                new ToastContentBuilder()
                 .AddHeader("重要通知", "这是一个标题", "此为副标题")
                .Show(toast =>
                {
                    toast.ExpirationTime = DateTime.Now.AddDays(2);
                }); // Not seei
            });
        }
        public DelegateCommand btn3Command
        {
            get => new DelegateCommand(() =>
            {
                //new ToastContentBuilder()
                //    .AddText("通知来了！")
                //    .AddAudio(new Uri("ms-winsoundevent:Notification.IM")) // 使用 IM 消息提示音
                //    .Show();


                new ToastContentBuilder()
                    .AddText("通知来了！")
       .AddAudio(new Uri("file:///D:/kamin.mp3")) // 确保使用 "file:///"
                    .Show();
            });
        }
        public DelegateCommand btn4Command
        {
            get => new DelegateCommand(() =>
            {
                new ToastContentBuilder()
                .AddText("正在播放")
                .AddText("Sing About Love")
                .AddText("Among Us")
                       //
                       .AddAppLogoOverride(new Uri("D:\\桌面\\CloudTrade云商\\CloudTrade\\src\\CloudTrade.Host\\Resources\\Images\\avatar.jpg"), ToastGenericAppLogoCrop.Circle)
                        .AddInlineImage(new Uri("D:\\桌面\\CloudTrade云商\\CloudTrade\\test\\Host.Test\\Among Us_Sing About Love_4.jpg"))
                        .AddButton(
                            new ToastButton()
                            .SetContent("上一首")
                            .AddArgument("action", "1")
                            .SetBackgroundActivation()
                        ).AddButton(
                            new ToastButton()
                            .SetContent("暂停")
                            .AddArgument("action", "1")
                            .SetBackgroundActivation()
                        )
                        .AddButton(
                            new ToastButton()
                            .SetContent("下一首")
                            .AddArgument("action", "3")
                            .SetBackgroundActivation())
                    .Show();



            });
        }

        public DelegateCommand ViewACommand
        {
            get => new DelegateCommand(() =>
            {
                regionManager.RequestNavigate("MainRegion", "ViewA", x =>
                {
                    journal = x.Context.NavigationService.Journal;
                });
            });
        }
        public DelegateCommand ViewBCommand
        {
            get => new DelegateCommand(() =>
            {
                regionManager.RequestNavigate("MainRegion", "ViewB", x =>
                {
                    journal = x.Context.NavigationService.Journal;

                });
            });
        }

        public DelegateCommand ForwardCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    journal.GoForward();
                });
            }
        }
        public DelegateCommand BackCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    journal.GoBack();
                });
            }
        }

        protected override void Update(IList arg)
        {
            throw new NotImplementedException();
        }

        protected override void Delete()
        {
            throw new NotImplementedException();
        }

        protected override void Query()
        {
            throw new NotImplementedException();
        }

        protected override async void Insert(string arg)
        {
            
            MessageBox.Show(arg);
        }
    }
}



public abstract class BaseViewModel : BindableBase
{
    public  DelegateCommand<string> InsertCommand
    {
        get
        {
            return new DelegateCommand<string>(async(arg)=>Insert(arg));
        }
    }
    protected abstract void Insert(string arg);
    
    public DelegateCommand<IList> UpdateCommand
    {
        get
        {
            return new DelegateCommand<IList>((arg)=>Update(arg));
        }
    }
    protected abstract void Update(IList arg);
    public DelegateCommand DeleteCommand
    {
        get
        {
            return new DelegateCommand(Delete);
        }
    }
    protected abstract void Delete();

    public DelegateCommand QueryCommand
    {
        get
        {
            return new DelegateCommand(Query);
        }
    }
    protected abstract void Query();
}

