// ReSharper disable ReturnValueOfPureMethodIsNotUsed
namespace TopoSort.Tests;

public class TopologicalSorterTests : TopologicalSorterTestsBase
{
    [Test]
    public void Sort_NullEdges_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => TopologicalSorter.Ordered<int>(null!));
    }

    [Test]
    public void Sort_EmptyEdges_ReturnsEmpty()
    {
        IEnumerable<int> result = TopologicalSorter.Ordered(Enumerable.Empty<TopologicalSortEdge<int>>());
        Assert.IsEmpty(result);
    }

    [TestCase(1, 2)]
    [TestCase(2, 1)]
    public void Sort_OneEdgeWithTestCases_ReturnsSameOrder(int dependency, int dependent)
    {
        IEnumerable<int> result = TopologicalSorter.Ordered([
            new TopologicalSortEdge<int>(dependency, dependent),
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
            new TopologicalSortEdge<int>(1, 2),
            new TopologicalSortEdge<int>(2, 3),
            new TopologicalSortEdge<int>(3, 1),
        ]).ToList());
    }

    [Test]
    public void Sort_SelfCycle_ThrowsInvalidOperationException()
    {
        // We need at least 2 edges to trigger SortImpl
        Assert.Throws<InvalidOperationException>(() => TopologicalSorter.Ordered([
            new TopologicalSortEdge<int>(1, 1),
            new TopologicalSortEdge<int>(2, 3),
        ]).ToList());
    }

    [TestCaseSource(nameof(TestCases))]
    public void Sort_TestCases_ReturnsCorrectOrder(IEnumerable<TopologicalSortEdge<int>> edges, int[] expected)
    {
        var result = TopologicalSorter.Ordered(edges).ToList();

        Assert.AreEqual(expected, result);
    }

    [Test]
    public void SingleEdgeCycle_ShouldThrow()
    {
        TopologicalSortEdge<int>[] edges = [new(1, 1)];
        Assert.Throws<InvalidOperationException>(() => edges.Ordered().ToList());
    }
}