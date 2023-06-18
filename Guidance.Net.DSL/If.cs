using System.Runtime.CompilerServices;

namespace GuidanceNet.DSL;

public class If : BaseBlock
{
    public If(bool condition, [CallerArgumentExpression("condition")] string? conditionExpression = default)
    {
        ConditionExpression = conditionExpression;
    }

    private If()
    {
        
    }

    protected override string TagName { get; set; } = "if";

    // else elements
    protected ICollection<BaseElement> ElseElements { get; } = new List<BaseElement>();

    public string? ConditionExpression { get; set; }

    public static If Equals(string left, string right, [CallerArgumentExpression("left")] string? leftExpression = default, [CallerArgumentExpression("right")] string? rightExpression = default)
    {
        return new If {
            ConditionExpression = $"(== {leftExpression} {rightExpression})",
        };
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

    public override string ToString()
    {
        var body = Body;
        var elseBody = string.Join("", ElseElements);
        if (elseBody != "") {
            elseBody = $@"{{{{else}}}}{elseBody}";
        }

        return $@"{BlockOpen(body, ConditionExpression!)}{body}{elseBody}{BlockClose(body)}";
    }
}
