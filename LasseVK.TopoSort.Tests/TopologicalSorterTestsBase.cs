namespace LasseVK.TopoSort.Tests;

public abstract class TopologicalSorterTestsBase
{
    public static IEnumerable<TestCaseData> TestCases()
    {
        yield return new TestCaseData(
                new TopologicalSortEdge<int>[] {
                    new TopologicalSortEdge<int>(1, 2),
                    new TopologicalSortEdge<int>(2, 3),
                    new TopologicalSortEdge<int>(3, 4),
                },
                new[] { 1, 2, 3, 4 }
            ).SetName("Linear dependencies");

        yield return new TestCaseData(
                new TopologicalSortEdge<int>[] {
                    new TopologicalSortEdge<int>(1, 2),
                    new TopologicalSortEdge<int>(1, 3),
                    new TopologicalSortEdge<int>(2, 4),
                    new TopologicalSortEdge<int>(3, 4),
                },
                new[] { 1, 2, 3, 4 }
            ).SetName("Diamond dependencies");

        yield return new TestCaseData(
                new TopologicalSortEdge<int>[] {
                    new TopologicalSortEdge<int>(1, 2),
                    new TopologicalSortEdge<int>(3, 4),
                },
                new[] { 1, 3, 2, 4 }
            ).SetName("Disconnected dependencies");
    }

}