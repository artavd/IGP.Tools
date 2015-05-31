namespace IGP.Tools.IO
{
    using SBL.Common.Annotations;

    public interface IPortCreator
    {
        bool CanBeCreatedFrom([NotNull] string portName);

        [NotNull]
        IPort CreatePort([NotNull] string portName, [CanBeNull] string parameters = null);
    }
}