using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace GuidanceNet.DSL;

public class Variable : BaseElement
{
    private readonly Func<string> getter;
    private readonly string name;

    public Variable(Expression<Func<string?>> getter)
    {
        this.getter = () => getter.Compile().Invoke() ?? "";
        name = ((MemberExpression)getter.Body).Member.Name;
    }

    public Variable(string? name, [CallerArgumentExpression("name")] string? variableName = default)
    {
        this.name = variableName;
    }

    public override string ToString()
    {
        return $@"{{{{{name}}}}}";
    }
}