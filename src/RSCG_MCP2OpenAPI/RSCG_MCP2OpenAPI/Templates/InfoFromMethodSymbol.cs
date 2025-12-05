
using Microsoft.CodeAnalysis;

namespace RSCG_MCP2OpenAPI.Templates;
    public class InfoFromMethodSymbol
{

    public InfoFromMethodSymbol(IMethodSymbol methodSymbol, InfoFromClassSymbol classParent)
    {


        ClassName = classParent.ClassName;
        FullClassName = classParent.FullClassName;

        if (classParent.Namespace != null) {
            Namespace = classParent.Namespace;
        }

        IsStatic = methodSymbol.IsStatic;
        MethodName = methodSymbol.Name;
        var methodParams = methodSymbol.Parameters;
        //switch (methodParams.Length)
        //{
        //    case 0:
        //        ShouldBeGet = true;
        //        break;
        //    default:
        //        ShouldBeGet = false;
        //        break;
        //}
        
        argParams = string.Join(", ",methodParams.Select(it=>it.Type.ToDisplayString(NullableFlowState.MaybeNull) +" "+ it.Name)); 
        callParamsRecord = string.Join(", ", methodParams.Select(it => "value."+it.Name));
        HasParams = methodParams.Length> 0;
        
    }

    public bool HasParams { get; private set; }
    public string argParams { get; private set; }
    public string callParamsRecord { get; private set; }
    //public bool ShouldBeGet { get; private set; }
    public bool IsStatic { get;private set; }

    public string? Namespace {  get; private set; }
    public string FullClassName { get; private set;  }
    public string ClassName { get; private set; }
    public string MethodName { get; private set;  }
}
