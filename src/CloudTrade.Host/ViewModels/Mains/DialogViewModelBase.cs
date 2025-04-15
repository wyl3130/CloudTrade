using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Mains
{
    public class DialogViewModelBase : BindableBase, IDialogAware
    {
        public string Title { get; set; } = "标题";
        //public event Action<IDialogResult> RequestClose;
        public DialogCloseListener RequestClose => throw new NotImplementedException();

        /// <summary>
        /// 是否可以关闭
        /// </summary>
        /// <returns></returns>
        public virtual bool CanCloseDialog()
        {
            return true;
        }


        /// <summary>
        /// 关闭时执行
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void OnDialogClosed()
        {

        }
        /// <summary>
        /// 打开时执行
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnDialogOpened(IDialogParameters parameters)
        {

        }
        protected virtual void CloseDialog(ButtonResult buttonResult, IDialogParameters dialogParameters = null) 
        {
       
        }
    }
}
