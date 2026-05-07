namespace TopoSort;

public static class TopologicalSortExtensions
{
    extension<T>(IEnumerable<TopologicalSortDependency<T>> dependencies)
        where T : notnull
    {
        public IEnumerable<T> Ordered(IEqualityComparer<T>? comparer = null) => new TopologicalSorter().Ordered(dependencies, comparer);
    }

    extension<T>(IEnumerable<ValueTuple<T, T>> dependencies)
        where T : notnull
    {
        public IEnumerable<T> TopoOrdered(IEqualityComparer<T>? comparer = null) => new TopologicalSorter().Ordered(dependencies.Select(d => new TopologicalSortDependency<T>(d.Item1, d.Item2)), comparer);
    }

    extension<T>(IEnumerable<Tuple<T, T>> dependencies)
        where T : notnull
    {
        public IEnumerable<T> TopoOrdered(IEqualityComparer<T>? comparer = null) => new TopologicalSorter().Ordered(dependencies.Select(d => new TopologicalSortDependency<T>(d.Item1, d.Item2)), comparer);
    }
}