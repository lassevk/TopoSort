namespace TopoSort;

public static class TopologicalSortExtensions
{
    extension<T>(IEnumerable<TopologicalSortDependency<T>> dependencies)
        where T : notnull
    {
        public IEnumerable<T> Ordered(IEqualityComparer<T>? equalityComparer = null, IComparer<T>? comparer = null) => TopologicalSorter.Ordered(dependencies, equalityComparer, comparer);
    }

    extension<T>(IEnumerable<ValueTuple<T, T>> dependencies)
        where T : notnull
    {
        public IEnumerable<T> TopoOrdered(IEqualityComparer<T>? equalityComparer = null, IComparer<T>? comparer = null)
            => TopologicalSorter.Ordered(dependencies.Select(d => new TopologicalSortDependency<T>(d.Item1, d.Item2)), equalityComparer, comparer);
    }

    extension<T>(IEnumerable<Tuple<T, T>> dependencies)
        where T : notnull
    {
        public IEnumerable<T> TopoOrdered(IEqualityComparer<T>? equalityComparer = null, IComparer<T>? comparer = null)
            => TopologicalSorter.Ordered(dependencies.Select(d => new TopologicalSortDependency<T>(d.Item1, d.Item2)), equalityComparer, comparer);
    }
}