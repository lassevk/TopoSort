namespace LasseVK.TopoSort.Tests;

public class TopologicalSortExtensions : TopologicalSorterTestsBase
{
    [TestCaseSource(nameof(TestCases))]
    public void Ordered_OnTopologicalSortDependencyCollection_WithTestCases(IEnumerable<TopologicalSortEdge<int>> edges, int[] expected)
    {
        var result = edges.Ordered().ToList();
        Assert.AreEqual(expected, result);
    }

    [TestCaseSource(nameof(TestCases))]
    public void Ordered_OnValueTupleCollection_WithTestCases(IEnumerable<TopologicalSortEdge<int>> edges, int[] expected)
    {
        IEnumerable<ValueTuple<int, int>> input = edges.Select(dep => new ValueTuple<int, int>(dep.Dependency, dep.Dependent));
        var result = input.TopoOrdered().ToList();
    }

    [TestCaseSource(nameof(TestCases))]
    public void Ordered_OnTupleCollection_WithTestCases(IEnumerable<TopologicalSortEdge<int>> edges, int[] expected)
    {
        IEnumerable<Tuple<int, int>> input = edges.Select(dep => new Tuple<int, int>(dep.Dependency, dep.Dependent));
        var result = input.TopoOrdered().ToList();
    }
}