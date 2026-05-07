namespace TopoSort;

public readonly record struct TopologicalSortDependency<T>(T Dependency, T Dependent);