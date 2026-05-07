namespace TopoSort;

internal static class TopologicalSorter
{
    public static IEnumerable<T> Ordered<T>(IEnumerable<TopologicalSortEdge<T>> edges, IEqualityComparer<T>? equalityComparer = null, IComparer<T>? comparer = null)
        where T : notnull
    {
        if (edges is null)
        {
            throw new ArgumentNullException(nameof(edges));
        }

        var all = edges.ToList();
        return all.Count switch
        {
            0 => []
          , 1 => [all[0].Dependency, all[0].Dependent]
          , _ => OrderedImpl(all, equalityComparer ?? EqualityComparer<T>.Default, comparer ?? Comparer<T>.Default)
        };
    }

    private static IEnumerable<T> OrderedImpl<T>(List<TopologicalSortEdge<T>> edges, IEqualityComparer<T> equalityComparer, IComparer<T> comparer)
        where T : notnull
    {
        var graph = new Dictionary<T, List<T>>(equalityComparer);
        var inputEdgeCount = new Dictionary<T, int>(equalityComparer);

        foreach (TopologicalSortEdge<T> dep in edges)
        {
#if NET8_0_OR_GREATER
            inputEdgeCount.TryAdd(dep.Dependency, 0);
            inputEdgeCount.TryAdd(dep.Dependent, 0);
#else
            TryAddDegree(inputEdgeCount, dep.Dependency);
            TryAddDegree(inputEdgeCount, dep.Dependent);
#endif

            if (!graph.TryGetValue(dep.Dependency, out List<T>? outboundEdges))
            {
                outboundEdges = new List<T>();
                graph[dep.Dependency] = outboundEdges;
            }

            outboundEdges.Add(dep.Dependent);
            inputEdgeCount[dep.Dependent]++;
        }

        var vertices = new Queue<T>();
        foreach (KeyValuePair<T, int> kvp in inputEdgeCount)
        {
            if (kvp.Value == 0)
            {
                vertices.Enqueue(kvp.Key);
            }
        }

        int outputCount = 0;
        while (vertices.Count > 0)
        {

#if NET8_0_OR_GREATER
            T[] batch = vertices.ToArray();
            batch.Sort(comparer.Compare);
#else
            T[] batch = vertices.OrderBy(x => x, comparer).ToArray();
#endif

            vertices.Clear();

            foreach (T item in batch)
            {
                yield return item;

                outputCount++;

                if (graph.TryGetValue(item, out List<T>? neighbors))
                {
                    foreach (T v in neighbors)
                    {
                        inputEdgeCount[v]--;
                        if (inputEdgeCount[v] == 0)
                        {
                            vertices.Enqueue(v);
                        }
                    }
                }
            }
        }

        if (outputCount != inputEdgeCount.Count)
        {
            throw new InvalidOperationException("Cycle detected in edge dependencies.");
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