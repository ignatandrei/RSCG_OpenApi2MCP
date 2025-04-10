
using Microsoft.OpenApi.Models;

namespace RSCG_OpenApi2MCP;
internal class FunctionToGenerate
{
    internal readonly string key;
    internal readonly KeyValuePair<OperationType, OpenApiOperation> op;

    public FunctionToGenerate(string key,KeyValuePair<OperationType, OpenApiOperation> op)
	{
        this.key = key;
        this.op = op;
    }
    public string UrlWithParameters
    {
        get
        {
            //if(op.Value.Parameters.Count == 0)    
            return key;
        }
    }
    public string QueryString()
    {
        if(!HasParameters()) return "";
        List<string> parametersNotInUrl = [];
        foreach (var param in op.Value.Parameters)
        {
            if (param.In == ParameterLocation.Query)
            {
                parametersNotInUrl.Add(param.Name);
            }
        }
        if(parametersNotInUrl.Count == 0) return "";
        var str = parametersNotInUrl.Select(it => it + "={" + it+"}");
        return "?"+string.Join("&", str); 
    }
    public string Description
    {
        get
        {
            if(op.Value == null) return FullDisplayName;
            var desc= op.Value.Description + " "+ op.Value.Summary;
            desc= desc?.Trim();
            if (string.IsNullOrWhiteSpace(desc)) return FullDisplayName;
            return desc!;
        }
    }
    int nrParameters()
    {
        return op.Value.Parameters?.Count??0;
    }
    bool HasParameters()
    {
        return nrParameters() > 0;
    }
    public string FullDisplayName 
    {
        get { return op.Key + "--"+ key + "-- Nr Parameters:"  + nrParameters(); }
    }
    public OperationType operationType
    {
        get { return op.Key; }
    }
    public string FuncName
    {
        get { return "MCP_"+ op.Key + key.Replace("/","_").Replace("{","_").Replace("}","_")+"_"; }
    }
    private string CSharpTypeFromSchema(OpenApiSchema? schema)
    {
        if (schema == null) return "string";
        switch(schema.Type)
        {
            case "string":
                return "string";
            case "integer":
                return "int";
            case "number":
                return "double";
            case "boolean":
                return "bool";
            case "array":
                if (schema.Items != null)
                    return CSharpTypeFromSchema(schema.Items) + "[]";
                return "string[]";                
            case "object":
                return "object";
            default:
                if (schema.Reference != null)
                {
                    return schema.Reference.Id;
                }
                else if (schema.Properties != null)
                {
                    return "object";
                }
                else
                {
                    return schema.Type;
                }
        }
    }
    public string ParamFunction() 
    {
        if (!HasParameters()) return "";
        var str = op.Value.Parameters.Select(it => CSharpTypeFromSchema(it.Schema) + " " + it.Name);
        return string.Join(",", str);
    }


}
