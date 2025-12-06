namespace RSCG_MCP2OpenAPI.Templates;

public class InfoFromClassSymbol
{
    public string? Namespace { get; private set; }
    public string FullClassName { get; private set; }
    public string ClassName { get; private set; }

    public InfoFromMethodSymbol[] infoFromMethodSymbols { get; private set; }
    public InfoFromClassSymbol(INamedTypeSymbol classParent)
    {
        ClassName = classParent.Name;
        FullClassName = classParent.Name;
        if(classParent.ContainingNamespace != null && !classParent.ContainingNamespace.IsGlobalNamespace)
        {
            Namespace = classParent.ContainingNamespace.ToDisplayString();
            FullClassName = Namespace + "." + FullClassName;
        }
        infoFromMethodSymbols = classParent
           .GetMembers()
           .OfType<IMethodSymbol>()
           .Where(m =>
               m.GetAttributes().Any(a => a.AttributeClass != null && a.AttributeClass.ToDisplayString().Contains("McpServerTool")) &&
               m.MethodKind == MethodKind.Ordinary
               )
           .Select(it=>new InfoFromMethodSymbol(it,this))
           .ToArray();
    }
    public int NrMethods => infoFromMethodSymbols?.Length ??0;

    internal string? GenerateAPI()
    {
        Class2OpenAPI class2OpenAPI = new Class2OpenAPI(this);
        return class2OpenAPI.Render();
    }
}
