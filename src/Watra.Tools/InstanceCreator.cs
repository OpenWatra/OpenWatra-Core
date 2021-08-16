// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Tools
{
    /// <inheritdoc/>
    public class InstanceCreator<T> : IInstanceCreator<T>
        where T : class, new()
    {
        /// <inheritdoc/>
        public T Create()
        {
            return new T();
        }
    }
}
