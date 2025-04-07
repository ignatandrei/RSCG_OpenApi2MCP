
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
    public string FuncName
    {
        get { return "MCP"+key.Replace("/","_").Replace("{","_").Replace("}","_")+"_"+op.Key; }
    }
}
