namespace TopoSort.Tests;

public class TopologicalValueExtensionsTests
{
    [TestCase(2, 1, 1, 2)]
    [TestCase(1, 2, 2, 1)]
    public void DependsOn_WithTestCases_ReturnsCorrectDependency(int value, int inputDependency, int expectedDependency, int expectedDependent)
    {
        TopologicalSortDependency<int> dependency = value.DependsOn(inputDependency);
        Assert.AreEqual(expectedDependency, dependency.Dependency);
        Assert.AreEqual(expectedDependent, dependency.Dependent);
    }

    [TestCase(1, 2, 1, 2)]
    [TestCase(2, 1, 2, 1)]
    public void WithDependent_WithTestCases_ReturnsCorrectDependency(int value, int inputDependent, int expectedDependency, int expectedDependent)
    {
        TopologicalSortDependency<int> dependency = value.WithDependent(inputDependent);
        Assert.AreEqual(expectedDependency, dependency.Dependency);
        Assert.AreEqual(expectedDependent, dependency.Dependent);
    }
}