namespace IGP.Tools.IO
{
    using SBL.Common.Annotations;

    public interface IPortFactory
    {
        [NotNull]
        IPort CreatePort([NotNull] string portName, [CanBeNull] string parameters = null);
    }
}