using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace GuidanceNet.DSL;

public class Generate : BaseElement
{
    public Generate(Expression<Func<string>> setter)
    {
        Setter = setter.Compile();
        VariableName = ((MemberExpression)setter.Body).Member.Name;
    }
        
    public Generate(string? variable, [CallerArgumentExpression("variable")] string? variableName = default)
    {
        VariableName = variableName;
    }

    public string? VariableName { get; set; }

    public Func<string> Setter { get; set; }

    public override string ToString()
    {
        return $"{{{{gen '{VariableName}'}}}}";
    }
}