using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TopoSort;

public static class TopologicalSortDependency
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TopologicalSortDependency<T> Create<T>(T dependency, T dependent) => new(dependency, dependent);
}

#if NET8_0_OR_GREATER
[DebuggerDisplay("{Dependency} -> {Dependent}")]
public readonly record struct TopologicalSortDependency<T>(T Dependency, T Dependent)
{
    public override string ToString() => $"{Dependency} -> {Dependent}";
}
#else
[DebuggerDisplay("{Dependency} -> {Dependent}")]
public readonly struct TopologicalSortDependency<T>
{
    public TopologicalSortDependency(T dependency, T dependent)
    {
        Dependency = dependency;
        Dependent = dependent;
    }

    public T Dependency { get; }
    public T Dependent { get; }

    public override string ToString() => $"{Dependency} -> {Dependent}";
}
#endif