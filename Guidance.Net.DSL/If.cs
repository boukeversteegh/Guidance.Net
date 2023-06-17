using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace GuidanceNet.DSL;

public class If : BaseBlock
{
    protected override string TagName { get; set; } = "if";
        
    // else elements
    protected ICollection<BaseElement> ElseElements { get; } = new List<BaseElement>();

    public If(Expression<Func<bool>> condition)
    {
        Condition = condition.Compile();
        VariableName = ((MemberExpression)condition.Body).Member.Name;
    }
        
    public If(bool condition, [CallerArgumentExpression("condition")]string? variableName = default)
    {
        Condition = () => condition;
        VariableName = variableName;
    }
        
    public If Then(params BaseElement[] elements)
    {
        foreach (var element in elements) {
            Add(element);
        }

        return this;
    }
        
    public If Else(params BaseElement[] elements)
    {
        foreach (var element in elements) {
            ElseElements.Add(element);
        }

        return this;
    }

    public string? VariableName { get; set; }

    protected Func<bool> Condition { get; set; }

    public override string ToString()
    {
        var body = Body;
        var elseBody = string.Join("", ElseElements);
        if (elseBody != "") {
            elseBody = $@"{{{{else}}}}{elseBody}";
        }

        return $@"{BlockOpen(body, VariableName!)}{body}{elseBody}{BlockClose(body)}";
    }
}