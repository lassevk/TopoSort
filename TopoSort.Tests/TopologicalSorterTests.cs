namespace TopoSort.Tests;

public class TopologicalSorterTests
{
    [Test]
    public void Sort_NullDependencies_ThrowsArgumentNullException()
    {
        var sorter = new TopologicalSorter();
        Assert.Throws<ArgumentNullException>(() => sorter.Sort<int>(null!));
    }
}