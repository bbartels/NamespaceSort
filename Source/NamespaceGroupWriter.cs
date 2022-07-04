namespace NamespaceSort;
using System.Text;

internal class NamespaceGroupWriter
{
    private readonly NamespaceGroupContainer _container;
    public NamespaceGroupWriter(NamespaceGroupContainer container) => _container = container;

    public string Write()
    {
        var sb = new StringBuilder();
        foreach (var group in _container)
        {
            foreach (var usings in group)
            {
                sb.AppendLine(usings.Name.ToString());
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
