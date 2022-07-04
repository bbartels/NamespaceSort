namespace NamespaceSort;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections;

class NamespaceGroupContainer : IEnumerable<IEnumerable<UsingDirectiveSyntax>>
{
    private readonly NamespaceGroupConfiguration _configuration;
    private readonly IReadOnlyDictionary<NamespaceGroupNode, List<UsingDirectiveSyntax>> _namespaceGroups;

    public NamespaceGroupContainer(NamespaceGroupConfiguration config,
        Dictionary<NamespaceGroupNode, List<UsingDirectiveSyntax>> namespaceGroups)
        => (_configuration, _namespaceGroups) = (config, namespaceGroups);

    public IEnumerator<IEnumerable<UsingDirectiveSyntax>> GetEnumerator()
        => _configuration.Groups.Where(_namespaceGroups.ContainsKey).Select(x => _namespaceGroups[x]).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
