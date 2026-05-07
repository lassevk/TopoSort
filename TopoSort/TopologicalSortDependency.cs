using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TopoSort;

public static class TopologicalSortDependency
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TopologicalSortDependency<T> Create<T>(T dependency, T dependent) => new(dependency, dependent);
}

[DebuggerDisplay("{Dependency} -> {Dependent}")]
public readonly record struct TopologicalSortDependency<T>(T Dependency, T Dependent)
{
    public override string ToString() => $"{Dependency} -> {Dependent}";
}