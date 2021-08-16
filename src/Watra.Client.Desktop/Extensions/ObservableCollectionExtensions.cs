// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Tools.Extensions
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Extension methods for <see cref="IObservableCollection{T}"/>.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Adds all items of type <typeparamref name="T"/> from <paramref name="range"/> to <paramref name="observableCollection"/>.
        /// </summary>
        /// <typeparam name="T">Type of the collection to add to <paramref name="observableCollection"/>. Do not use as type filter.</typeparam>
        public static void AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                observableCollection.Add(item);
            }
        }
    }
}
