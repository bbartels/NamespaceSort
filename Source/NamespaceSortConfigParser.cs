using System.Text.RegularExpressions;

namespace NamespaceSort;


public class NamespaceSortConfigParser
{
    private const char _separator = ';';
    private const char _matchAll = '*';
    private const char _navigation = '.';
    private static Regex TrimWhitespaces() => new Regex(@"\s+");

    public NamespaceGroupConfiguration Parse(string input)
        => new(SanitizeInput(input).Split(_separator).Select(x => ParseGroup(x)));

    private NamespaceGroupNode ParseGroup(ReadOnlySpan<char> groupString) => groupString switch
    {
        [_matchAll, .. ReadOnlySpan<char>] => new MatchAllNamespaceGroupNode(1),
        [.. ReadOnlySpan<char> t] when t.IndexOf(_navigation) is int index and >= 0
            => new IntermediaryIdentifierNamespaceGroupNode(new(t[..index].ToString()), ParseGroup(t[(index + 1)..])),
        [.. ReadOnlySpan<char> t] => new EndIdentifierNamespaceGroupNode(new(t.ToString()))
    };

    private static string SanitizeInput(string input) => TrimWhitespaces().Replace(input, string.Empty);
}
