namespace TopoSort;

internal static class TopologicalSorter
{
    public static IEnumerable<T> Ordered<T>(IEnumerable<TopologicalSortDependency<T>> dependencies, IEqualityComparer<T>? equalityComparer = null, IComparer<T>? comparer = null)
        where T : notnull
    {
        if (dependencies is null)
        {
            throw new ArgumentNullException(nameof(dependencies));
        }

        var all = dependencies.ToList();
        return all.Count switch
        {
            0 => []
          , 1 => [all[0].Dependency, all[0].Dependent]
          , _ => OrderedImpl(all, equalityComparer ?? EqualityComparer<T>.Default, comparer ?? Comparer<T>.Default)
        };
    }

    private static IEnumerable<T> OrderedImpl<T>(List<TopologicalSortDependency<T>> dependencies, IEqualityComparer<T> equalityComparer, IComparer<T> comparer)
        where T : notnull
    {
        var adjacencyList = new Dictionary<T, List<T>>(equalityComparer);
        var inDegree = new Dictionary<T, int>(equalityComparer);

        foreach (TopologicalSortDependency<T> dep in dependencies)
        {
#if NET8_0_OR_GREATER
            inDegree.TryAdd(dep.Dependency, 0);
            inDegree.TryAdd(dep.Dependent, 0);
#else
            TryAddDegree(inDegree, dep.Dependency);
            TryAddDegree(inDegree, dep.Dependent);
#endif

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

#if NET8_0_OR_GREATER
            T[] batch = queue.ToArray();
            batch.Sort(comparer.Compare);
#else
            T[] batch = queue.OrderBy(x => x, comparer).ToArray();
#endif

            queue.Clear();

            foreach (T item in batch)
            {
                yield return item;

                outputCount++;

                if (adjacencyList.TryGetValue(item, out List<T>? neighbors))
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
        }

        if (outputCount != inDegree.Count)
        {
            throw new InvalidOperationException("Cycle detected in dependencies.");
        }
    }

#if !NET8_0_OR_GREATER
    // ReSharper disable CanSimplifyDictionaryLookupWithTryAdd
    private static void TryAddDegree<T>(Dictionary<T, int> inDegree, T depDependent)
        where T : notnull
    {
        if (!inDegree.ContainsKey(depDependent))
        {
            inDegree.Add(depDependent, 0);
        }
    }
    // ReSharper restore CanSimplifyDictionaryLookupWithTryAdd
#endif
}