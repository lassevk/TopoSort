namespace TopoSort.Tests;

public class TopologicalSortVertexExtensionsTests
{
    [TestCase(2, 1, 1, 2)]
    [TestCase(1, 2, 2, 1)]
    public void DependsOn_WithTestCases_ReturnsCorrectDependency(int value, int inputDependency, int expectedDependency, int expectedDependent)
    {
        TopologicalSortEdge<int> edge = value.DependsOn(inputDependency);
        Assert.AreEqual(expectedDependency, edge.Dependency);
        Assert.AreEqual(expectedDependent, edge.Dependent);
    }

    [TestCase(1, 2, 1, 2)]
    [TestCase(2, 1, 2, 1)]
    public void WithDependent_WithTestCases_ReturnsCorrectDependency(int value, int inputDependent, int expectedDependency, int expectedDependent)
    {
        TopologicalSortEdge<int> edge = value.WithDependent(inputDependent);
        Assert.AreEqual(expectedDependency, edge.Dependency);
        Assert.AreEqual(expectedDependent, edge.Dependent);
    }
}