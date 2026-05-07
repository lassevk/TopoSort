namespace TopoSort.Tests;

public class TopologicalSorterTests
{
    [Test]
    public void Sort_NullDependencies_ThrowsArgumentNullException()
    {
        var sorter = new TopologicalSorter();
        Assert.Throws<ArgumentNullException>(() => sorter.Sort<int>(null!));
    }

    [Test]
    public void Sort_EmptyDependencies_ReturnsEmpty()
    {
        var sorter = new TopologicalSorter();
        IEnumerable<int> result = sorter.Sort(Enumerable.Empty<TopologicalSortDependency<int>>());
        Assert.IsEmpty(result);
    }

    [TestCase(1, 2)]
    [TestCase(2, 1)]
    public void Sort_OneDependencyWithTestCases_ReturnsSameOrder(int dependency, int dependent)
    {
        var sorter = new TopologicalSorter();
        IEnumerable<int> result = sorter.Sort([
            new TopologicalSortDependency<int>(dependency, dependent),
        ]);

        Assert.AreEqual(new[]
        {
            dependency, dependent,
        }, result);
    }
}