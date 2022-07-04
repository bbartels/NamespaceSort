namespace NamespaceSort;

public class NamespaceGroupConfiguration
{
    private static NamespaceGroupNode Default = new MatchAllNamespaceGroupNode(1);

    public IReadOnlyCollection<NamespaceGroupNode> Groups { get; }

    public NamespaceGroupConfiguration(IEnumerable<NamespaceGroupNode> groups)
        => Groups = EnsureContainsCatchAll(groups);

    private IReadOnlyCollection<NamespaceGroupNode> EnsureContainsCatchAll(IEnumerable<NamespaceGroupNode> groups)
        => (groups.Any(x => x is MatchAllNamespaceGroupNode) ? groups : groups.Append(Default)).ToList();

    public NamespaceGroupNode Match(ReadOnlySpan<string> input)
    {
        MatchResult? result = null;
        NamespaceGroupNode? mostSpecificGroup = null;

        foreach (var group in Groups)
        {
            if (group.Match(input) is { Success: true } match && match.Depth > (result?.Depth ?? -1))
            {
                result = match;
                mostSpecificGroup = group;
            }
        }

        return mostSpecificGroup ?? throw new Exception("This should not happen");
    }
}
