using System.Windows.Input;
using SBL.Common.Annotations;

namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    internal interface IRibbonViewModel
    {
        ICommand StartEmulatorsCommand { [NotNull] get; }

        ICommand StopEmulatorsCommand { [NotNull] get; }
    }
}