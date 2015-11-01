namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using System.Collections.Generic;
    using IGP.Tools.DeviceEmulatorManager.Services;

    internal interface IRibbonViewModel
    {
        IEnumerable<RibbonCommand> Commands { get; }
    }
}