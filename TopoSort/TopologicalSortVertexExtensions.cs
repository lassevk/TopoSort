namespace TopoSort;

/// <summary>
/// Adds extension methods to T to build edges used with <see cref="TopologicalSortExtensions"/>.
/// </summary>
public static class TopologicalSortVertexExtensions
{
    /// <param name="dependent">The vertex value that the edge will point to.</param>
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

    /// <param name="dependency">The vertex value that the edge will originate from.</param>
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