
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
}
