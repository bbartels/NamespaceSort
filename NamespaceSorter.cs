namespace NamespaceSort;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal class NamespaceSorter
{
    private readonly NamespaceGroupConfiguration _groupCollection;

    public NamespaceSorter(NamespaceGroupConfiguration groupCollection)
        => _groupCollection = groupCollection;

    public NamespaceGroupContainer Sort(SyntaxList<UsingDirectiveSyntax> usingStatements)
    {
        var result = usingStatements.Select(x => new
        {
            Group = _groupCollection.Match(x.Name.ToString().Split('.')),
            Namespace = x
        }).GroupBy(x => x.Group, x => x.Namespace)
         .ToDictionary(x => x.Key, x => x.OrderBy(x => x.Name.ToString()).ToList());

        return new NamespaceGroupContainer(_groupCollection, result);
    }
}
