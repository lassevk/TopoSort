// ReSharper disable ReturnValueOfPureMethodIsNotUsed
namespace TopoSort.Tests;

public class TopologicalSorterTests : TopologicalSorterTestsBase
{
    [Test]
    public void Sort_NullDependencies_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => TopologicalSorter.Ordered<int>(null!));
    }

    [Test]
    public void Sort_EmptyDependencies_ReturnsEmpty()
    {
        IEnumerable<int> result = TopologicalSorter.Ordered(Enumerable.Empty<TopologicalSortDependency<int>>());
        Assert.IsEmpty(result);
    }

    [TestCase(1, 2)]
    [TestCase(2, 1)]
    public void Sort_OneDependencyWithTestCases_ReturnsSameOrder(int dependency, int dependent)
    {
        IEnumerable<int> result = TopologicalSorter.Ordered([
            new TopologicalSortDependency<int>(dependency, dependent),
        ]);

        Assert.AreEqual(new[]
        {
            dependency, dependent,
        }, result);
    }


    [Test]
    public void Sort_CycleDetected_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => TopologicalSorter.Ordered([
            new TopologicalSortDependency<int>(1, 2),
            new TopologicalSortDependency<int>(2, 3),
            new TopologicalSortDependency<int>(3, 1),
        ]).ToList());
    }

    [Test]
    public void Sort_SelfCycle_ThrowsInvalidOperationException()
    {
        // We need at least 2 dependencies to trigger SortImpl
        Assert.Throws<InvalidOperationException>(() => TopologicalSorter.Ordered([
            new TopologicalSortDependency<int>(1, 1),
            new TopologicalSortDependency<int>(2, 3),
        ]).ToList());
    }

    [TestCaseSource(nameof(TestCases))]
    public void Sort_TestCases_ReturnsCorrectOrder(IEnumerable<TopologicalSortDependency<int>> dependencies, int[] expected)
    {
        var result = TopologicalSorter.Ordered(dependencies).ToList();

        Assert.AreEqual(expected, result);
    }
}