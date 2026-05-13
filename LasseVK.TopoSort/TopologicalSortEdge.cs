using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TopoSort;

/// <summary>
/// Static class for helper methods for creating <see cref="TopologicalSortEdge{T}"/>.
/// </summary>
public static class TopologicalSortEdge
{
    /// <summary>
    /// Creates a <see cref="TopologicalSortEdge{T}"/> with the specified <paramref name="dependency"/> and <paramref name="dependent"/>.
    /// </summary>
    /// <param name="dependency">
    /// The dependency of the edge.
    /// </param>
    /// <param name="dependent">
    /// The dependent of the edge, which depends on <paramref name="dependency"/>.
    /// </param>
    /// <typeparam name="T">
    /// The type of values of the dependency and dependent.
    /// </typeparam>
    /// <returns>
    /// A <see cref="TopologicalSortEdge{T}"/> with the specified <paramref name="dependency"/> and <paramref name="dependent"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TopologicalSortEdge<T> Create<T>(T dependency, T dependent) => new(dependency, dependent);
}

/// <summary>
/// Represents an edge in a directed acyclic graph (DAG), consisting of a dependency and a dependent.
/// </summary>
/// <typeparam name="T">
/// The type of values of the dependency and dependent.
/// </typeparam>
#if NET8_0_OR_GREATER
[DebuggerDisplay("{Dependency} -> {Dependent}")]
public readonly record struct TopologicalSortEdge<T>(T Dependency, T Dependent)
{
    /// <inheritdoc/>
    public override string ToString() => $"{Dependency} -> {Dependent}";
}
#else
[DebuggerDisplay("{Dependency} -> {Dependent}")]
public readonly struct TopologicalSortEdge<T>
{
    /// <summary>
    /// Creates a new instance of <see cref="TopologicalSortEdge{T}"/>.
    /// </summary>
    /// <param name="dependency">
    /// The dependency of the edge.
    /// </param>
    /// <param name="dependent">
    /// The dependent of the edge, which depends on <paramref name="dependency"/>.
    /// </param>
    public TopologicalSortEdge(T dependency, T dependent)
    {
        Dependency = dependency;
        Dependent = dependent;
    }

    /// <summary>
    /// The dependency of the edge.
    /// </summary>
    public T Dependency { get; }

    /// <summary>
    /// The dependent of the edge, which depends on <see cref="Dependency"/>.
    /// </summary>
    public T Dependent { get; }

    /// <inheritdoc/>
    public override string ToString() => $"{Dependency} -> {Dependent}";
}
#endif