# TopoSort

This package adds methods and types for topological sorting, a graph algorithm that determines a valid ordering of vertices in a directed acyclic graph (DAG) such that for every directed edge (u, v), vertex u comes before vertex v in the ordering.

It provides a `TopologicalSorter` class that can be used to sort dependencies in a project or any other scenario where dependencies need to be resolved in a specific order.

***Note:*** Portions of this library was added by AI, given that I have an IDE that uses AI-style autocompletion.
There is *however*, no portion of it not vetted by me.

## Installation

You can install the package from the command line, using `dotnet`:

```bash
dotnet add package MedallionTopologicalSort
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
indicating which values follows from which other values (dependents depends on dependencies).

You can specify the dependencies in a variety of ways, including using the `TopologicalSortDependency` class or tuples.

```csharp
var dependencies = [
    new TopologicalSortDependency<int>(1, 2),
    new TopologicalSortDependency<int>(2, 3),
    new TopologicalSortDependency<int>(3, 4),
    new TopologicalSortDependency<int>(4, 5),
];
var sorter = new TopologicalSorter<int>();
var sorted = sorter.Sort(dependencies);
// sorted will be [1, 2, 3, 4, 5]
```

When using `ValueTuple<T1, T2>` or `Tuple<T1, T2>`, in order to not coflict with existing
sorting methods in .NET, here the name of the method will be `TopoOrdered`, like this:

```csharp
var dependencies = new ValueTuple<int, int>[]
{
    (1, 2),
    (2, 3),
    (3, 4),
    (4, 5),
}
var sorter = new TopologicalSorter<int>();
var sorted = sorter.Sort(dependencies);
// sorted will be [1, 2, 3, 4, 5]
```

# Notes and remarks

* Values will not be duplicated, and each value in the dependency graph is produced just once
  in the final result. There is no support for having duplicate values. **However**, given
  that a `IEqualityComparer<T>` is involved, this can be mimicked with a custom made implementation.
* There is no built-in support for vertices not involved in any dependencies. Loose values
  will have to be handled outside of these ordering methods.
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

The optional `IComparer<T>` comparer instance that can be specified allows the individual values, when they are unrelated,
to have an order.

In other words, it is possible for the above example to output the values as `[3, 1, 4, 2]`, if a comparer is
specified that reverses the default ordering of the integers. However, it will *not* impact the ordering of
the dependencies, so there is no way using the comparer to get the `2` or the `4` to appear before the `1` or the `3`,
as this is a guarantee by the topological sort algorithm that the dependencies will always appear before their dependents.
