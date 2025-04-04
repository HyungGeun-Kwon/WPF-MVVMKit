using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMKit.Dialogs
{
    public interface IDialogAware
    {
        void OnDialogOpened(DialogParameters parameters);
        void OnDialogClosed();
    }
}
