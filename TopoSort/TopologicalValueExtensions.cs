namespace TopoSort;

public static class TopologicalValueExtensions
{
    extension<T>(T value)
    {
        public TopologicalSortDependency<T> DependsOn(T dependency) => new(dependency, value);
        public TopologicalSortDependency<T> WithDependent(T dependent) => new(value, dependent);
    }
}