using System.Xml;

namespace TopoSort;

public class TopologicalSorter
{
    public IEnumerable<T> Sort<T>(IEnumerable<TopologicalSortDependency<T>> dependencies)
    {
        ArgumentNullException.ThrowIfNull(dependencies);

        var all = dependencies.ToList();

        throw new NotImplementedException();
    }
}