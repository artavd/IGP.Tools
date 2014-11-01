namespace IGP.Tools.IO.Implementation
{
    using System.Collections.Generic;
    using SBL.Common;

    internal sealed class FilterChain : IPortFilter
    {
        private readonly IList<IPortFilter> _filters = new List<IPortFilter>();

        public bool IsEnabled { get; set; }

        public byte[] Filter(byte[] data)
        {
            Contract.ArgumentIsNotNull(data, () => data);

            if (!IsEnabled)
            {
                return data;
            }

            byte[] buffer = data;
            foreach (var filter in _filters)
            {
                buffer = filter.Filter(buffer);
                if (buffer == null)
                {
                    return null;
                }
            }

            return buffer;
        }

        public void AddFilter(IPortFilter filter)
        {
            Contract.ArgumentIsNotNull(filter, () => filter);

            _filters.Add(filter);
        }
    }
}