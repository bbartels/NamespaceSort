namespace NamespaceSort;
using System;

public class NamespaceIdentifier
{
    public string Name { get; }

    public NamespaceIdentifier(string identifier)
    {
        if (identifier is not { Length: > 0 })
        {
            throw new InvalidParseException($"Invalid namespace identifier: {identifier}");
        }
        Name = identifier;
    }
}

public record struct MatchResult(bool Success, int Depth);

public abstract class NamespaceGroupNode
{
    public MatchResult Match(ReadOnlySpan<string> @namespace) => Match(@namespace, new(false, 0));
    internal abstract MatchResult Match(ReadOnlySpan<string> @namespace, MatchResult result);
}

public class IntermediaryIdentifierNamespaceGroupNode : NamespaceGroupNode
{
    public NamespaceIdentifier Identifier { get; }
    public NamespaceGroupNode Next { get; }

    public IntermediaryIdentifierNamespaceGroupNode(NamespaceIdentifier identifier, NamespaceGroupNode next)
        => (Identifier, Next) = (identifier, next);

    internal override MatchResult Match(ReadOnlySpan<string> @namespace, MatchResult result) => @namespace switch
    {
        [string identifier, .. ReadOnlySpan<string> t] when identifier == Identifier.Name
            => Next.Match(t, result with { Depth = result.Depth + 1 }),
        _ => result with { Depth = result.Depth + 1, Success = false }
    };
}

public abstract class EndNamespaceGroupNode : NamespaceGroupNode { }

public class EndIdentifierNamespaceGroupNode : EndNamespaceGroupNode
{
    public NamespaceIdentifier Identifier { get; }

    public EndIdentifierNamespaceGroupNode(NamespaceIdentifier identifier) => Identifier = identifier;

    internal override MatchResult Match(ReadOnlySpan<string> @namespace, MatchResult result) => @namespace switch
    {
        [string identifier] when identifier == Identifier.Name => result with { Success = true, Depth = result.Depth + 1 },
        _ => result with { Success = false }
    };
}

public class MatchAllNamespaceGroupNode : EndNamespaceGroupNode
{
    public int Rank { get; }
    public MatchAllNamespaceGroupNode(int rank) => (Rank) = (rank);

    internal override MatchResult Match(ReadOnlySpan<string> @namespace, MatchResult result)
        => result with { Success = true, Depth = result.Depth };
}
