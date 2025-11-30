
namespace RSCG_MCP2File;

public class MCP2File : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context
           .RegisterPostInitializationOutput(i =>
           {
               i.AddEmbeddedAttributeDefinition();
               i.AddSource("MCPExportToFile.g.cs", @"
                namespace MCP2File
                {
                    [global::Microsoft.CodeAnalysis.EmbeddedAttribute]
                    internal class AddMCPExportToFile: global::System.Attribute {} 
                }");
           });

        var mcpClasses= context.SyntaxProvider.ForAttributeWithMetadataName(
            "MCP2File.AddMCPExportToFile",
            static (node, _) => node is ClassDeclarationSyntax,
            static (context, _) =>
            {
                INamedTypeSymbol? classSymbol = null;
                var classDeclaration =context.TargetNode as ClassDeclarationSyntax;
                if (classDeclaration is null)
                    return classSymbol;

                classSymbol = context.TargetSymbol as INamedTypeSymbol;
                return classSymbol;
            })
             .Where(static m => m is not null)
            .Select(static (m, _) => m!)
            ;

        context.RegisterSourceOutput(mcpClasses,(ctx,cls)=>
        {
            var classSymbol = cls;
            var funcs = cls
                    .GetMembers()
                    .OfType<IMethodSymbol>()
                    .Where(m => m.GetAttributes().Any(a => a.AttributeClass != null && a.AttributeClass.ToDisplayString().Contains("McpServerTool")))
                    .ToArray();
            ;
        });


    }
}
