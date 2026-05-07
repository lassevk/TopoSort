namespace TopoSort.Tests;

public abstract class TopologicalSorterTestsBase
{
    public static IEnumerable<TestCaseData> TestCases()
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

}