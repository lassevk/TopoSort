namespace TopoSort;

public static class TopologicalSortExtensions
{
    extension<T>(IEnumerable<TopologicalSortDependency<T>> dependencies)
    {
        public IEnumerable<T> Ordered() => new TopologicalSorter().Sort(dependencies);
    }

    extension<T>(IEnumerable<ValueTuple<T, T>> dependencies)
    {
        public IEnumerable<T> Ordered() => new TopologicalSorter().Sort(dependencies.Select(d => new TopologicalSortDependency<T>(d.Item1, d.Item2)));
    }

    extension<T>(IEnumerable<Tuple<T, T>> dependencies)
    {
        public IEnumerable<T> Ordered() => new TopologicalSorter().Sort(dependencies.Select(d => new TopologicalSortDependency<T>(d.Item1, d.Item2)));
    }
}