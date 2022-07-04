namespace NamespaceSort.Tests;

using Xunit;

public class NamespaceGroupConfigurationTests
{
    private const string _input = "System; System.Collections.* ; System.Collections.Concurrent ; Namespace.* ; *";

    private static IEnumerable<object[]> GetPermutations()
    {
        var input = _input.Split(';').Select(x => x.Trim());
        var res = Permute(input, Enumerable.Empty<string>()).Select(x => new[] { string.Join(';', x).GetConfig() }).ToArray();
        return res;

        IEnumerable<string[]> Permute(IEnumerable<string> reminder, IEnumerable<string> prefix)
            => !reminder.Any() ? new [] { prefix.ToArray() } :
                reminder.SelectMany((c, i) => Permute(reminder.Take(i).Concat(reminder.Skip(i + 1)).ToArray(), prefix.Append(c)));
    }

    [Theory]
    [MemberData(nameof(GetPermutations))]
    public void MatchOneSpecificIdentifier(NamespaceGroupConfiguration config)
    {
        var result = config.Match(new string[] { "System" });
        result.IsEndNamespaceGroup("System");
    }

    [Theory]
    [MemberData(nameof(GetPermutations))]
    public void MatchOneSpecificIdentifierOnMatchAllAfterSpecificIdentifierGroup(NamespaceGroupConfiguration config)
    {
        var result = config.Match(new string[] { "Namespace" });
        var node = result.IsIntermediaryNamespaceGroup("Namespace");
        node.Next.IsMatchAllGroup();
    }

    [Theory]
    [MemberData(nameof(GetPermutations))]
    public void MatchThreeSpecificIdentifiers(NamespaceGroupConfiguration config)
    {
        var result = config.Match(new string[] { "System", "Collections", "Concurrent" });
        var node = result.IsIntermediaryNamespaceGroup("System");
        node = node.Next.IsIntermediaryNamespaceGroup("Collections");
        node.Next.IsEndNamespaceGroup("Concurrent");
    }

    [Theory]
    [MemberData(nameof(GetPermutations))]
    public void MatchOneSpecificIdentifierFollowedByOneSpecificIdentifierMatchedByMatchAllGroupNode(NamespaceGroupConfiguration config)
    {
        var result = config.Match(new string[] { "System", "Collections" });
        var node = result.IsIntermediaryNamespaceGroup("System");
        node = node.Next.IsIntermediaryNamespaceGroup("Collections");
        node.Next.IsMatchAllGroup();
    }

    [Theory]
    [MemberData(nameof(GetPermutations))]
    public void MatchTwoSpecificIdentifiersFollowedByMatchAnyIdentifier(NamespaceGroupConfiguration config)
    {
        var result = config.Match(new string[] { "System", "Collections", "Generic" });
        var node = result.IsIntermediaryNamespaceGroup("System");
        node = node.Next.IsIntermediaryNamespaceGroup("Collections");
        node.Next.IsMatchAllGroup();
    }
}

