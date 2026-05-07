namespace TopoSort.Tests;

public class TopologicalSortDependencyTests
{
    [TestCase(1, 2)]
    [TestCase(2, 1)]
    public void Create_WithTestCases(int dependency, int dependent)
    {
        var dep = TopologicalSortDependency.Create(dependency, dependent);
        Assert.AreEqual(dependency, dep.Dependency);
        Assert.AreEqual(dependent, dep.Dependent);
    }

    [TestCase(1, 2, "1 -> 2")]
    [TestCase(2, 1, "2 -> 1")]
    public void ToString_WithTestCases(int dependency, int dependent, string expected)
    {
        var dep = TopologicalSortDependency.Create(dependency, dependent);
        Assert.AreEqual(expected, dep.ToString());
    }
}