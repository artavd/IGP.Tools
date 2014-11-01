namespace IGP.Tools.IO
{
    using SBL.Common.Annotations;

    public interface IPortFilter
    {
        bool IsEnabled { get; set; }

        [CanBeNull]
        byte[] Filter([NotNull] byte[] data);
    }
}