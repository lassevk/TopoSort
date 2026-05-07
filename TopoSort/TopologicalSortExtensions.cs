namespace TopoSort;

public static class TopologicalSortExtensions
{
    extension<T>(IEnumerable<TopologicalSortDependency<T>> dependencies)
        where T : notnull
    {
        public IEnumerable<T> Ordered() => new TopologicalSorter().Ordered(dependencies);
    }

    extension<T>(IEnumerable<ValueTuple<T, T>> dependencies)
        where T : notnull
    {
        public IEnumerable<T> TopoOrdered() => new TopologicalSorter().Ordered(dependencies.Select(d => new TopologicalSortDependency<T>(d.Item1, d.Item2)));
    }

    extension<T>(IEnumerable<Tuple<T, T>> dependencies)
        where T : notnull
    {
        public IEnumerable<T> TopoOrdered() => new TopologicalSorter().Ordered(dependencies.Select(d => new TopologicalSortDependency<T>(d.Item1, d.Item2)));
    }
}