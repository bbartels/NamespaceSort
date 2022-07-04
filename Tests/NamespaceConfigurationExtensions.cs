namespace NamespaceSort.Tests;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class TestUtilExtensions
{
    public static NamespaceGroupConfiguration GetConfig(this string input)
        => new NamespaceSortConfigParser().Parse(input);

    public static SyntaxList<UsingDirectiveSyntax> GetSyntax(this string source)
        => CSharpSyntaxTree.ParseText(source).GetCompilationUnitRoot().Usings;

    public static IEnumerable<SyntaxList<UsingDirectiveSyntax>> GetGroupSyntax(this string source)
        => source.Split("-").Select(x => x.GetSyntax());
}
