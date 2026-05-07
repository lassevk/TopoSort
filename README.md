# TopoSort

This package adds methods and types for topological sorting, a graph algorithm that determines a valid ordering of
vertices in a directed acyclic graph (DAG) such that for every directed edge (u, v), vertex u comes before vertex v
in the ordering.

The package provides a set of extension methods that does this ordering.

***Note:*** Portions of this library were added by AI, given that I have an IDE that uses AI-style autocompletion.
There is *however*, no portion of it not vetted by me.

## Installation

You can install the package from the command line, using `dotnet`:

```bash
dotnet add package TopoSort
```

Or you can use your favorite IDE which should have a Nuget package manager built in.

## Framework support

The package supports the following .NET versions and standards:

* .NET 8.0 (until november 10, 2026)
* .NET 9.0 (until november 10, 2026)
* .NET 10.0 (until november 14, 2028)
* .NET Standard 2.0
* .NET Standard 2.1

This follows the official supported versions policies from Microsoft:

* [.NET Standard Versions](https://dotnet.microsoft.com/en-us/platform/dotnet-standard)
* [The official .NET support policy](https://dotnet.microsoft.com/en-us/platform/support/policy)

## Usage

The project relies on the code providing the added methods with a list of dependencies,
indicating which values follow from which other values (dependents depends on dependencies).

You can specify the dependencies in a variety of ways, including using the `TopologicalSortEdge` class or tuples.

```csharp
var dependencies = [
    new TopologicalSortEdge<int>(1, 2),
    new TopologicalSortEdge<int>(2, 3),
    new TopologicalSortEdge<int>(3, 4),
    new TopologicalSortEdge<int>(4, 5),
];
var sorted = dependencies.Ordered().ToList();
// sorted will be [1, 2, 3, 4, 5]
```

When using `ValueTuple<T1, T2>` or `Tuple<T1, T2>`, in order to not conflict with existing
sorting methods in .NET, the name of the method will be `TopoOrdered`, like this:

```csharp
var dependencies = new ValueTuple<int, int>[]
{
    (1, 2),
    (2, 3),
    (3, 4),
    (4, 5),
}
var sorted = dependencies.TopoOrdered().ToList();
// sorted will be [1, 2, 3, 4, 5]
```
# IEqualityComparer and IComparer

The methods all have two optional arguments:

* `IEqualityComparer<T> equalityComparer` which will be used to determine if vertex values being used are the same.
  This defaults to `EqualityComparer<T>.Default` if no specific equality comparer is being specified.
* `IComparer<T> comparer` which will be used to determine the order of vertices that are otherwise unrelated. This defaults to
  `Comparer<T>.Default` if no specific comparer is being specified. See more about this at the bottom of the notes
  section.

# Notes and remarks

* Vertices will not be duplicated, and each vertex in the dependency graph are produced just once
  in the final result. There is no support for having duplicate vertices. **However**, given
  that a `IEqualityComparer<T>` is involved, this can be mimicked with a custom made implementation.
* There is no built-in support for vertices not involved in any dependencies. Loose vertices
  will have to be handled outside of these ordering methods.
* Cycles in the graph will result in an `InvalidOperationException` being thrown. This involves multi-vertex
  cycles, such as `1-->2, 2-->3, 3-->1`, as well a self-referencing edges, such as `1-->1`.
* Distinct dependencies, ie. dependencies that have no bearing on each other, will result
  in an output that contains all the dependencies together, before all the dependents together.
  In other words, the following set of dependencies:

  ```
  1 --> 2
  3 --> 4
  ```
  
  will result in the following output:

  ```
  1, 3, 2, 4
  ^+-^  ^+-^
   |    |
   |    +- dependents
   +- dependencies

The way these values are output is that the algorithm finds first the group of values that is `1` and `3`,
then outputs those, and then afterwards it finds `2` and `4` and outputs those.

Each of these sets contain vertices that are otherwise unrelated (ie. there is no relationship between
`1` and `3`), and in this case, the optional `IComparer<T>` instance, if specified, can be used to determine
the final output ordering of all the values in a given set.

Given an `IComparer<int>` implementation that results in a reversed order, it would thus be possible to
get the following output instead:

```
3, 1, 4, 2
```

however, the edge dependencies are what guarantees that `1` comes before `2` and that `3` comes before `4`,
and nothing the `IComparer<T>` instance does will change that.
