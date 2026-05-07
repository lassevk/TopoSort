namespace TopoSort.Tests;

public class TopologicalSortExtensions : TopologicalSorterTestsBase
{
    [TestCaseSource(nameof(TestCases))]
    public void Ordered_OnTopologicalSortDependencyCollection_WithTestCases(IEnumerable<TopologicalSortDependency<int>> dependencies, int[] expected)
    {
        var result = dependencies.Ordered().ToList();
        Assert.AreEqual(expected, result);
    }

    [TestCaseSource(nameof(TestCases))]
    public void Ordered_OnValueTupleCollection_WithTestCases(IEnumerable<TopologicalSortDependency<int>> dependencies, int[] expected)
    {
        IEnumerable<ValueTuple<int, int>> input = dependencies.Select(dep => new ValueTuple<int, int>(dep.Dependency, dep.Dependent));
        var result = input.TopoOrdered().ToList();
    }

    [TestCaseSource(nameof(TestCases))]
    public void Ordered_OnTupleCollection_WithTestCases(IEnumerable<TopologicalSortDependency<int>> dependencies, int[] expected)
    {
        IEnumerable<Tuple<int, int>> input = dependencies.Select(dep => new Tuple<int, int>(dep.Dependency, dep.Dependent));
        var result = input.TopoOrdered().ToList();
    }
}