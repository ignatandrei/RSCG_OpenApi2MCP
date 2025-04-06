using Microsoft.CodeAnalysis;
using Microsoft.OpenApi.Readers;
using System;

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
            var fileJson = Path.Combine(csproj, "obj", name, ".json");
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
        if (!json.IsSuccess)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor("RSCG00"+((int)json.Status), "Error", "Error: {0}", "RSCG", DiagnosticSeverity.Error, true),
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
                new DiagnosticDescriptor("RSCG00" + ((int)FileJsonEnum.JSONWithErrors), "Error", "Error: {0}", "RSCG", DiagnosticSeverity.Error, true),
                Location.None,
                FileJsonEnum.JSONWithErrors.ToString() + "--" + diagnostic.Errors[0].Message));

            return;
        }

    }
}
