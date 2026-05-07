namespace TopoSort.Tests;

public class TopologicalSorterTests
{
    [Test]
    public void Sort_NullDependencies_ThrowsArgumentNullException()
    {
        var sorter = new TopologicalSorter();
        Assert.Throws<ArgumentNullException>(() => sorter.Ordered<int>(null!));
    }

    [Test]
    public void Sort_EmptyDependencies_ReturnsEmpty()
    {
        var sorter = new TopologicalSorter();
        IEnumerable<int> result = sorter.Ordered(Enumerable.Empty<TopologicalSortDependency<int>>());
        Assert.IsEmpty(result);
    }

    [TestCase(1, 2)]
    [TestCase(2, 1)]
    public void Sort_OneDependencyWithTestCases_ReturnsSameOrder(int dependency, int dependent)
    {
        var sorter = new TopologicalSorter();
        IEnumerable<int> result = sorter.Ordered([
            new TopologicalSortDependency<int>(dependency, dependent),
        ]);

        Assert.AreEqual(new[]
        {
            dependency, dependent,
        }, result);
    }

    private static IEnumerable<TestCaseData> SortTestCases()
    {
        yield return new TestCaseData(
            new TopologicalSortDependency<int>[] {
                new TopologicalSortDependency<int>(1, 2),
                new TopologicalSortDependency<int>(2, 3),
                new TopologicalSortDependency<int>(3, 4),
            },
            new[] { 1, 2, 3, 4 }
        ).SetName("Linear dependencies");

        yield return new TestCaseData(
            new TopologicalSortDependency<int>[] {
                new TopologicalSortDependency<int>(1, 2),
                new TopologicalSortDependency<int>(1, 3),
                new TopologicalSortDependency<int>(2, 4),
                new TopologicalSortDependency<int>(3, 4),
            },
            new[] { 1, 2, 3, 4 }
        ).SetName("Diamond dependencies");

        yield return new TestCaseData(
            new TopologicalSortDependency<int>[] {
                new TopologicalSortDependency<int>(1, 2),
                new TopologicalSortDependency<int>(3, 4),
            },
            new[] { 1, 3, 2, 4 }
        ).SetName("Disconnected dependencies");
    }

    [Test]
    public void Sort_CycleDetected_ThrowsInvalidOperationException()
    {
        var sorter = new TopologicalSorter();
        Assert.Throws<InvalidOperationException>(() => sorter.Ordered([
            new TopologicalSortDependency<int>(1, 2),
            new TopologicalSortDependency<int>(2, 3),
            new TopologicalSortDependency<int>(3, 1),
        ]).ToList());
    }

    [Test]
    public void Sort_SelfCycle_ThrowsInvalidOperationException()
    {
        var sorter = new TopologicalSorter();
        // We need at least 2 dependencies to trigger SortImpl
        Assert.Throws<InvalidOperationException>(() => sorter.Ordered([
            new TopologicalSortDependency<int>(1, 1),
            new TopologicalSortDependency<int>(2, 3),
        ]).ToList());
    }

    [TestCaseSource(nameof(SortTestCases))]
    public void Sort_TestCases_ReturnsCorrectOrder(IEnumerable<TopologicalSortDependency<int>> dependencies, int[] expected)
    {
        var sorter = new TopologicalSorter();
        var result = sorter.Ordered(dependencies).ToList();

        Assert.AreEqual(expected, result);
    }
}