namespace TopoSort;

internal class TopologicalSorter
{
    public IEnumerable<T> Ordered<T>(IEnumerable<TopologicalSortDependency<T>> dependencies, IEqualityComparer<T>? comparer = null)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(dependencies);

        var all = dependencies.ToList();
        return all.Count switch
        {
            0 => []
          , 1 => [all[0].Dependency, all[0].Dependent]
          , _ => OrderedImpl(all, comparer ?? EqualityComparer<T>.Default)
        };
    }

    private static IEnumerable<T> OrderedImpl<T>(List<TopologicalSortDependency<T>> dependencies, IEqualityComparer<T> comparer)
        where T : notnull
    {
        var adjacencyList = new Dictionary<T, List<T>>(comparer);
        var inDegree = new Dictionary<T, int>(comparer);

        foreach (TopologicalSortDependency<T> dep in dependencies)
        {
            inDegree.TryAdd(dep.Dependency, 0);
            inDegree.TryAdd(dep.Dependent, 0);

            if (!adjacencyList.TryGetValue(dep.Dependency, out List<T>? neighbors))
            {
                neighbors = new List<T>();
                adjacencyList[dep.Dependency] = neighbors;
            }

            neighbors.Add(dep.Dependent);
            inDegree[dep.Dependent]++;
        }

        var queue = new Queue<T>();
        foreach (KeyValuePair<T, int> kvp in inDegree)
        {
            if (kvp.Value == 0)
            {
                queue.Enqueue(kvp.Key);
            }
        }

        int outputCount = 0;
        while (queue.Count > 0)
        {
            T u = queue.Dequeue();
            yield return u;

            outputCount++;

            if (adjacencyList.TryGetValue(u, out List<T>? neighbors))
            {
                foreach (T v in neighbors)
                {
                    inDegree[v]--;
                    if (inDegree[v] == 0)
                    {
                        queue.Enqueue(v);
                    }
                }
            }
        }

        if (outputCount != inDegree.Count)
        {
            throw new InvalidOperationException("Cycle detected in dependencies.");
        }
    }
}