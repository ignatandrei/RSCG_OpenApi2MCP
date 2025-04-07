
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
        get { 
            if(op.Value.Parameters.Count == 0)    
                return key;

            return "//TODO: add parameters interpolation";        
        }
    }
    public string Description
    {
        get
        {
            return "TODO: desc from comments: " + op.Value.Description;
        }
    }
    public string FullDisplayName
    {
        get { return op.Key + "--"+ key + "-- Nr Parameters:"  + (op.Value.Parameters?.Count??0); }
    }
    public OperationType operationType
    {
        get { return op.Key; }
    }
    public string FuncName
    {
        get { return "MCP"+key.Replace("/","_").Replace("{","_").Replace("}","_")+"_"+op.Key; }
    }
}
