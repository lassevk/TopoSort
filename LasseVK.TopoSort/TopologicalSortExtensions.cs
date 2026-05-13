namespace TopoSort;

/// <summary>
/// Adds extension methods to IEnumerable to perform topological sorting, and for creating edges.
/// </summary>
public static class TopologicalSortExtensions
{
    /// <param name="edges">
    /// The edge dependencies that are used to perform topological sorting.
    /// </param>
    /// <typeparam name="T">
    /// The type of values of the vertices of the edges.
    /// </typeparam>
    extension<T>(IEnumerable<TopologicalSortEdge<T>> edges)
        where T : notnull
    {
        /// <summary>
        /// Performs topological sorting on the collection of graph edges.
        /// </summary>
        /// <param name="equalityComparer">
        /// Optional `IEqualityComparer{T}` to use for comparing elements, used to ensure that when vertex values are specified in various
        /// edge dependencies, they are compared correctly. Defaults to `EqualityComparer{T}.Default`.
        /// </param>
        /// <param name="comparer">
        /// Optional `IComparer{T}` to use for comparing elements, used order vertex values that are otherwise unrelated, to ensure
        /// a consistent ordering. Defaults to `Comparer{T}.Default`.
        /// </param>
        /// <returns>
        /// A collection of vertices in topological order.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// edges is `null`.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The collection of edges contains a cycle.
        /// </exception>
        public IEnumerable<T> Ordered(IEqualityComparer<T>? equalityComparer = null, IComparer<T>? comparer = null) => TopologicalSorter.Ordered(edges, equalityComparer, comparer);
    }

    /// <param name="edges">
    /// The edge dependencies that are used to perform topological sorting.
    /// </param>
    /// <typeparam name="T">
    /// The type of values of the vertices of the edges.
    /// </typeparam>
    extension<T>(IEnumerable<ValueTuple<T, T>> edges)
        where T : notnull
    {
        /// <summary>
        /// Performs topological sorting on the collection of graph edges, as specified by a collection of `(T, T)` tuples.
        /// </summary>
        /// <param name="equalityComparer">
        /// Optional `IEqualityComparer{T}` to use for comparing elements, used to ensure that when vertex values are specified in various
        /// edge dependencies, they are compared correctly. Defaults to `EqualityComparer{T}.Default`.
        /// </param>
        /// <param name="comparer">
        /// Optional `IComparer{T}` to use for comparing elements, used order vertex values that are otherwise unrelated, to ensure
        /// a consistent ordering. Defaults to `Comparer{T}.Default`.
        /// </param>
        /// <returns>
        /// A collection of vertices in topological order.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// edges is `null`.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The collection of edges contains a cycle.
        /// </exception>
        public IEnumerable<T> TopoOrdered(IEqualityComparer<T>? equalityComparer = null, IComparer<T>? comparer = null)
            => TopologicalSorter.Ordered(edges.Select(d => new TopologicalSortEdge<T>(d.Item1, d.Item2)), equalityComparer, comparer);
    }

    /// <param name="edges">
    /// The edge dependencies that are used to perform topological sorting.
    /// </param>
    /// <typeparam name="T">
    /// The type of values of the vertices of the edges.
    /// </typeparam>
    extension<T>(IEnumerable<Tuple<T, T>> edges)
        where T : notnull
    {
        /// <summary>
        /// Performs topological sorting on the collection of edge dependencies, as specified by a collection of `(T, T)` tuples.
        /// </summary>
        /// <param name="equalityComparer">
        /// Optional `IEqualityComparer{T}` to use for comparing elements, used to ensure that when vertex values are specified in various
        /// edge dependencies, they are compared correctly. Defaults to `EqualityComparer{T}.Default`.
        /// </param>
        /// <param name="comparer">
        /// Optional `IComparer{T}` to use for comparing elements, used order vertex values that are otherwise unrelated, to ensure
        /// a consistent ordering. Defaults to `Comparer{T}.Default`.
        /// </param>
        /// <returns>
        /// A collection of vertices in topological order.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// edges is `null`.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The collection of edges contains a cycle.
        /// </exception>
        public IEnumerable<T> TopoOrdered(IEqualityComparer<T>? equalityComparer = null, IComparer<T>? comparer = null)
            => TopologicalSorter.Ordered(edges.Select(d => new TopologicalSortEdge<T>(d.Item1, d.Item2)), equalityComparer, comparer);
    }

    /// <param name="dependent">
    /// The vertex value that the edge will point to.
    /// </param>
    /// <typeparam name="T">
    /// The type of the values of the vertices of the created edge.
    /// </typeparam>
    extension<T>(T dependent)
    {
        /// <summary>
        /// Creates a <see cref="TopologicalSortEdge{T}"/> with the value as the dependent.
        /// </summary>
        /// <param name="dependency">
        /// The dependency for the vertex.
        /// </param>
        /// <returns>
        /// A <see cref="TopologicalSortEdge{T}"/> that represents an edge dependency from
        /// <paramref name="dependency"/> to dependent.
        /// </returns>
        /// <example>
        /// <code>
        /// 10.DependsOn(5)
        /// </code>
        ///
        /// produces a dependency on the form 5 --&gt; 10.
        /// </example>
        public TopologicalSortEdge<T> DependsOn(T dependency) => new(dependency, dependent);
    }

    /// <param name="dependency">
    /// The vertex value that the edge will originate from.
    /// </param>
    /// <typeparam name="T">
    /// The type of the values of the vertices of the created edge.
    /// </typeparam>
    extension<T>(T dependency)
    {
        /// <summary>
        /// Creates a <see cref="TopologicalSortEdge{T}"/> with the value as the dependency.
        /// </summary>
        /// <param name="dependent">
        /// The dependency for the vertex.
        /// </param>
        /// <returns>
        /// A <see cref="TopologicalSortEdge{T}"/> that represents an edge dependency from
        /// dependency to <paramref name="dependent"/>.
        /// </returns>
        /// <example>
        /// <code>
        /// 5.WithDependent(10)
        /// </code>
        ///
        /// produces a dependency on the form 5 --&gt; 10.
        /// </example>
        public TopologicalSortEdge<T> WithDependent(T dependent) => new(dependency, dependent);
    }
}