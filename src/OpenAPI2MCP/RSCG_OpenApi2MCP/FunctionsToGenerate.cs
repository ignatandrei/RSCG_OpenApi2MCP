using Microsoft.OpenApi.Models;

namespace RSCG_OpenApi2MCP;
internal class FunctionsToGenerate
{
    internal List<FunctionToGenerate> functions = [];
   public FunctionsToGenerate(OpenApiDocument document,string name)
   {
        if (document.Paths?.Count > 0)
        {
            foreach (var path in document.Paths)
            {
                var key = path.Key;
                if (path.Value.Operations?.Count > 0)
                {

                    functions.AddRange(path.Value.Operations.Select(it => new FunctionToGenerate(key, it)).ToArray());
                }
            }
        }

        Name = name;
    }
    public int Count=>functions.Count;

    public string Name { get; }

    public string TemplateToDisplay()
    {
        var template = new RSCG_OpenApi2MCP.Templates.AllFunctions(this);
        var result = template.Render();
        return result;
    }
    
}
