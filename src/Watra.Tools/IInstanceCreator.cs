// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Tools
{
    /// <summary>
    /// Create and initialize instances of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type for which objects shall be created.</typeparam>
    public interface IInstanceCreator<T>
        where T : class, new()
    {
        /// <summary>
        /// Returns a new instance of <typeparamref name="T"/>.
        /// </summary>
        T Create();
    }
}
