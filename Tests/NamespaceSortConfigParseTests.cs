using Xunit;

namespace NamespaceSort.Tests;

public class NamespaceSortConfigParseTests
{
    private readonly NamespaceSortConfigParser _configParser;

    public NamespaceSortConfigParseTests()
    {
        _configParser = new NamespaceSortConfigParser();
    }

    [Fact]
    public void InputWithSingleGroupNodeReturnsSingleEndGroupNode()
    {
        string input = "Namespace";
        var result = _configParser.Parse(input);

        Assert.Equal(2, result.Groups.Count);
        var nodeGroup = Assert.IsType<EndIdentifierNamespaceGroupNode>(result.Groups.First());
        Assert.Equal("Namespace", nodeGroup.Identifier.Name);
        Assert.IsType<MatchAllNamespaceGroupNode>(result.Groups.Skip(1).First());
    }

    [Fact]
    public void InputWithFullyQualifiedNamespaceReturnsChainOfIdentifierNodesEndingInEndGroupNode()
    {
        string input = "Namespace.Test";
        var result = _configParser.Parse(input);

        Assert.Equal(2, result.Groups.Count);
        var intermediaryNode = Assert.IsType<IntermediaryIdentifierNamespaceGroupNode>(result.Groups.First());
        Assert.Equal("Namespace", intermediaryNode.Identifier.Name);
        var endNode = Assert.IsType<EndIdentifierNamespaceGroupNode>(intermediaryNode.Next);
        Assert.Equal("Test", endNode.Identifier.Name);
        Assert.IsType<MatchAllNamespaceGroupNode>(result.Groups.Skip(1).First());
    }

    [Fact]
    public void InputWithMatchAllNodeAfterIdentifierNodeReturnsIdentifierNodeFollowedByMatchAllNode()
    {
        string input = "Namespace.*";
        var result = _configParser.Parse(input);

        Assert.Equal(2, result.Groups.Count);
        var intermediaryNode = Assert.IsType<IntermediaryIdentifierNamespaceGroupNode>(result.Groups.First());
        Assert.Equal("Namespace", intermediaryNode.Identifier.Name);
        Assert.IsType<MatchAllNamespaceGroupNode>(intermediaryNode.Next);
        Assert.IsType<MatchAllNamespaceGroupNode>(result.Groups.Skip(1).First());
    }

    [Fact]
    public void InputWithSingleTopLevelCatchAllGroupReturnsCatchAllNodeGroup()
    {
        string input = "*";
        var result = _configParser.Parse(input);

        var group = Assert.Single(result.Groups);
        Assert.IsType<MatchAllNamespaceGroupNode>(group);
    }

    [Theory]
    [InlineData("")]
    public void InvalidNamespaceSortConfigParseInputThrowsException(string input)
    {
        Assert.Throws<InvalidParseException>(() => _configParser.Parse(input));
    }
}