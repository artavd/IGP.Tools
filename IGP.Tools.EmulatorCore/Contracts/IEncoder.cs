namespace IGP.Tools.EmulatorCore
{
    using SBL.Common.Annotations;

    public interface IEncoder
    {
        [NotNull]
        byte[] Encode([NotNull] string source);

        [NotNull]
        string Decode([NotNull] byte[] data);
    }
}