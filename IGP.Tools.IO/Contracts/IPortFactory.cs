namespace IGP.Tools.IO
{
    using SBL.Common.Annotations;

    public interface IPortFactory
    {
        [NotNull]
        IPort CreatePort([NotNull] string portName, [NotNull] string parameters);
    }
}