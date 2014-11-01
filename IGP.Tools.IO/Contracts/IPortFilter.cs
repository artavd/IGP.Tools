namespace IGP.Tools.IO.Contracts
{
    using SBL.Common.Annotations;

    public interface IPortFilter
    {
        bool IsEnabled { get; set; }

        [CanBeNull]
        byte[] Filter([NotNull] byte[] data);
    }
}