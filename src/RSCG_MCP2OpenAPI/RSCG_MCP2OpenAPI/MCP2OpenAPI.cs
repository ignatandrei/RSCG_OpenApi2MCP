using RSCG_MCP2OpenAPI.Templates;

namespace RSCG_MCP2OpenAPI;

[Generator]
public class MCP2OpenAPI : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context
          .RegisterPostInitializationOutput(i =>
          {
              i.AddEmbeddedAttributeDefinition();
              i.AddSource("MCP2OpenAPI.g.cs", @"
                namespace MCP2OpenAPI
                {
                    [global::Microsoft.CodeAnalysis.EmbeddedAttribute]
                    [global::System.AttributeUsage(global::System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
                    internal class AddMCP2OpenApi: global::System.Attribute {} 
                }");

          });

        var nr = 0;
        var mcpClasses = context.SyntaxProvider.ForAttributeWithMetadataName(
            "MCP2OpenAPI.AddMCP2OpenApi",
            static (node, _) => node is ClassDeclarationSyntax,
            static (context, _) =>
            {
                INamedTypeSymbol? classSymbol = null;
                var classDeclaration = context.TargetNode as ClassDeclarationSyntax;
                if (classDeclaration is null)
                    return classSymbol;

                classSymbol = context.TargetSymbol as INamedTypeSymbol;
                return classSymbol;
            })
             .Where(static m => m is not null)
            .Select(static (m, _) => m!)
            .Collect()
            ;
        nr++;

        context.RegisterSourceOutput(mcpClasses, (ctx, clsArr) =>
        {
            if (clsArr.Length == 0)
                return;

            foreach (var cls in clsArr)
            {

                var res = GenerateFromClass(cls);
                if (res != null)
                {
                    ctx.AddSource($"{cls.Name}_ExportToFile.g.cs", res);

                }
            }

        });
    }

    private string? GenerateFromClass(INamedTypeSymbol cls)
    {

        
        var classInfo = new InfoFromClassSymbol(cls);
        return classInfo.GenerateAPI();
    }
    static bool IsVoidOrTask(ITypeSymbol returnType)
    {
        return returnType.SpecialType == SpecialType.System_Void ||
               returnType.ToDisplayString() == "System.Threading.Tasks.Task";
    }

}
