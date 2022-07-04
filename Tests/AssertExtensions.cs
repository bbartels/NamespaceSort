namespace NamespaceSort.Tests;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

internal static class AssertExtensions
{
    private class InvalidNodeException : Exception
    {
        private InvalidNodeException(string message) : base(message) { }

        public static InvalidNodeException InvalidIdentifierName(string actual, string expected)
            => new($"Invalid namespace identifier: {actual}, expected: {expected}");
    }

    public static IntermediaryIdentifierNamespaceGroupNode IsIntermediaryNamespaceGroup(
        this NamespaceGroupNode? node, string identifier)
        => Assert.IsType<IntermediaryIdentifierNamespaceGroupNode>(node) is var result
            && result.Identifier.Name != identifier
                ? throw InvalidNodeException.InvalidIdentifierName(identifier, result.Identifier.Name)
                : result;

    public static EndIdentifierNamespaceGroupNode IsEndNamespaceGroup(
        this NamespaceGroupNode? node, string identifier)
        => Assert.IsType<EndIdentifierNamespaceGroupNode>(node) is var result
            && result.Identifier.Name != identifier
                ? throw InvalidNodeException.InvalidIdentifierName(identifier, result.Identifier.Name)
                : result;

    public static MatchAllNamespaceGroupNode IsMatchAllGroup(this NamespaceGroupNode? node)
        => Assert.IsType<MatchAllNamespaceGroupNode>(node);

    public static void MatchesSortedNamespaces(this NamespaceGroupContainer? actual,
        IEnumerable<SyntaxList<UsingDirectiveSyntax>> expected)
    {
        Assert.NotNull(actual);
        Assert.Equal(expected.Count(), actual.Count());
        foreach(var groupPair in actual.Zip(expected))
        {
            var result = groupPair.First.Zip(groupPair.Second);
            foreach((var expectedUsing, var actualUsing) in result)
            {
                Assert.Equal(expectedUsing.Name.ToString(), actualUsing.Name.ToString());
            }
        }
    }
}
