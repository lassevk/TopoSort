namespace LasseVK.TopoSort;

/// <summary>
/// Internal class that implements the topological sorting algorithm.
/// </summary>
internal static class TopologicalSorter
{
    /// <summary>
    /// Performs topological sorting on the specified collection of edges.
    /// </summary>
    /// <typeparam name="T">The type of the vertices in the graph.</typeparam>
    /// <param name="edges">The collection of directed edges representing dependencies.</param>
    /// <param name="equalityComparer">An optional equality comparer for vertices.</param>
    /// <param name="comparer">An optional comparer to provide a stable order for unrelated vertices.</param>
    /// <returns>An enumerable of vertices in topological order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="edges"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when a cycle is detected in the dependencies.</exception>
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
          , _ => OrderedImpl(all, equalityComparer ?? EqualityComparer<T>.Default, comparer ?? Comparer<T>.Default)
        };
    }

    /// <summary>
    /// Implementation of the topological sort using Kahn's algorithm.
    /// </summary>
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