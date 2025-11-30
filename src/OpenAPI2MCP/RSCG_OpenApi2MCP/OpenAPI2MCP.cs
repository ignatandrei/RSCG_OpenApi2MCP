using Microsoft.CodeAnalysis;
using Microsoft.OpenApi.Readers;
using System;
using System.Threading.Tasks;
/*
 * dotnet "D:\MyPackages\.nuget\packages\microsoft.extensions.apidescription.server\9.0.3\build\../tools/dotnet-getdocument.dll" --assembly "D:\gth\RSCG_OpenApi2MCP\src\OA2MCP_TestMe\bin\Debug\net9.0\OA2MCP_TestMe.dll" --file-list "obj\OA2MCP_TestMe.OpenApiFiles.cache" --framework ".NETCoreApp,Version=v9.0" --output "D:\gth\RSCG_OpenApi2MCP\src\OA2MCP_TestMe\obj" --project "OA2MCP_TestMe" --assets-file "D:\gth\RSCG_OpenApi2MCP\src\OA2MCP_TestMe\obj\project.assets.json" --platform "AnyCPU"
 */
namespace RSCG_OpenApi2MCP;
enum FileJsonEnum
{
    None,
    FoundJSON,
    NotFoundCsprojDir,
    NotFoundCsprojFile,
    TooManyCsprojFile,
    NotFoundObj,
    NotFoundJSON,
    JSONWithErrors,
}
class FileJson
{
    public FileJson(FileJsonEnum status, string path)
    {
        Path = path;
        Status = status;
    }
    public FileJson(string path) : this(FileJsonEnum.FoundJSON, path)
    {
    }
    public bool IsSuccess => Status == FileJsonEnum.FoundJSON;
    public string Path { get; private set; } 
    public FileJsonEnum Status { get; private set; } 
    
}
[Generator]
public class OpenAPI2MCP : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {

        var pathObj = context.AnalyzerConfigOptionsProvider.Select((provider, ct) =>

        {

            if (!(provider.GlobalOptions.TryGetValue("build_property.ProjectDir", out var csproj)))
                return new FileJson(FileJsonEnum.NotFoundCsprojDir,"");
#pragma warning disable RS1035
            var csprojs = Directory.GetFiles(csproj, "*.csproj", SearchOption.TopDirectoryOnly);
            
            if (csprojs.Length == 0)
                return new FileJson(FileJsonEnum.NotFoundCsprojDir,csproj);
            if(csprojs.Length>1)
                return new FileJson(FileJsonEnum.TooManyCsprojFile,csproj);
            var csprojFile= csprojs[0];
            var nameCsproj = csprojFile.Split(Path.DirectorySeparatorChar).Last();
            var name = nameCsproj.Split('.').First();
            var obj = Path.Combine(csproj, "obj");
            if (!Directory.Exists(obj))
                return new FileJson(FileJsonEnum.NotFoundObj,obj);
            var fileJson = Path.Combine(csproj, "obj", name+ ".json");
            if (!File.Exists(fileJson))
                return new FileJson(FileJsonEnum.NotFoundJSON,fileJson);
            return new FileJson(fileJson);

#pragma warning restore RS1035

        }
        );

        context.RegisterSourceOutput(pathObj, RegisterData);
        

    }

    private void RegisterData(SourceProductionContext context, FileJson json)
    {
        string name = "localSwagger";
        var template = new RSCG_OpenApi2MCP.Templates.RegisterCallData(name);
        var result = template.Render();
        context.AddSource("RegisterCallData", result);

        if (!json.IsSuccess)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor("RSCG00"+((int)json.Status), "Problem", "Problem: {0}", "RSCG", DiagnosticSeverity.Warning, true),
                Location.None,
                json.Status.ToString() + "--"+ json.Path));
            return;
        }
        using var streamReader = new StreamReader(json.Path);
        var reader = new OpenApiStreamReader();
        var document = reader.Read(streamReader.BaseStream, out var diagnostic);
        if (diagnostic.Errors.Count > 0)        
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor("RSCG00" + ((int)FileJsonEnum.JSONWithErrors), "Problem", "Problem: {0}", "RSCG", DiagnosticSeverity.Warning, true),
                Location.None,
                FileJsonEnum.JSONWithErrors.ToString() + "--" + diagnostic.Errors[0].Message));

            return;
        }
        FunctionsToGenerate functions = new(document,name);
        context.AddSource("FunctionsToGenerate", functions.TemplateToDisplay());

    }
}
