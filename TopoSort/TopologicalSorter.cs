using System.Xml;

namespace TopoSort;

public class TopologicalSorter
{
    public IEnumerable<T> Sort<T>(IEnumerable<TopologicalSortDependency<T>> dependencies, IEqualityComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(dependencies);

        var all = dependencies.ToList();
        return all.Count switch
        {
            0 => []
          , 1 => [all[0].Dependency, all[0].Dependent]
          , _ => throw new NotImplementedException()
        };
    }
}