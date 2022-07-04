namespace NamespaceSort;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class InvalidParseException : Exception
{
    public InvalidParseException(string message) : base(message) { }
}

public class NamespaceImportParser
{
    public SyntaxList<UsingDirectiveSyntax> Parse(string input)
        => CSharpSyntaxTree.ParseText(input).GetCompilationUnitRoot().Usings;
}
