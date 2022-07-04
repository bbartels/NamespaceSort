namespace NamespaceSort.Tests;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

public class NamespaceSortingTests
{
    private const string _program =
        """
        using System;
        using System.IO;
        using Microsoft.Foo;
        using Microsoft.Foo.Bar;
        using Microsoft.Bar.Foo;
        using SampleNamespace;
        """;

    private static SyntaxList<UsingDirectiveSyntax> _syntax = _program.GetSyntax();

    [Fact]
    public void MatchSystemNamespaceInFirstGroupFollowedByMatchAllInSecondGroup()
    {
        var sorted = new NamespaceSorter("System ; *".GetConfig()).Sort(_syntax);

        var expected = """
            using System;
            -
            using Microsoft.Bar.Foo;
            using Microsoft.Foo;
            using Microsoft.Foo.Bar;
            using SampleNamespace;
            using System.IO;
            """.GetGroupSyntax();

        sorted.MatchesSortedNamespaces(expected);
    }

    [Fact]
    public static void MatchAnySystemNamespaceInFirstGroupFollowedByMatchAllInSecondGroup()
    {
        var sorted = new NamespaceSorter("System.* ; *".GetConfig()).Sort(_syntax);

        var expected = """
            using System;
            using System.IO;
            -
            using Microsoft.Bar.Foo;
            using Microsoft.Foo;
            using Microsoft.Foo.Bar;
            using SampleNamespace;
            """.GetGroupSyntax();

        sorted.MatchesSortedNamespaces(expected);
    }

    [Fact]
    public static void MatchAnySystemNamespaceInFirstGroupFollowedByDefaultMatchAllInSecondGroup()
    {
        var sorted = new NamespaceSorter("System.*".GetConfig()).Sort(_syntax);

        var expected = """
            using System;
            using System.IO;
            -
            using Microsoft.Bar.Foo;
            using Microsoft.Foo;
            using Microsoft.Foo.Bar;
            using SampleNamespace;
            """.GetGroupSyntax();

        sorted.MatchesSortedNamespaces(expected);
    }

    [Fact]
    public static void MatchAnySpecificNamespaceFollowedByMatchAnyNamespaceFollowedByMatchAnySystemNamespace()
    {
        var sorted = new NamespaceSorter("Microsoft.Foo.* ; * ; System.*".GetConfig()).Sort(_syntax);

        var expected = """
            using Microsoft.Foo;
            using Microsoft.Foo.Bar;
            -
            using Microsoft.Bar.Foo;
            using SampleNamespace;
            -
            using System;
            using System.IO;
            """.GetGroupSyntax();

        sorted.MatchesSortedNamespaces(expected);
    }

    [Fact]
    public static void MatchTwoSpecificNamespacesFollowedByMatchAnyNamespaceFollowedByMatchAnySystemNamespace()
    {
        var sorted = new NamespaceSorter("Microsoft.Bar.Foo.* ; Microsoft.Foo.* ; * ; System.*".GetConfig()).Sort(_syntax);

        var expected = """
            using Microsoft.Bar.Foo;
            -
            using Microsoft.Foo;
            using Microsoft.Foo.Bar;
            -
            using SampleNamespace;
            -
            using System;
            using System.IO;
            """.GetGroupSyntax();

        sorted.MatchesSortedNamespaces(expected);
    }
}

