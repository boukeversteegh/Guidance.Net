using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace GuidanceNet.DSL;

public class GuidanceBuilder
{
    public static GuidanceProgram Guidance(params BaseElement[] elements)
    {
        var program = new GuidanceProgram();
        foreach (var element in elements)
        {
            program.Add(element);
        }

        return program;
    }
    
    public static System SystemRole(params BaseElement[] elements)
    {
        return new System(elements);
    }

    public static Assistant AssistantRole(params BaseElement[] elements)
    {
        return new Assistant(elements);
    }
    
    public static User UserRole(params BaseElement[] elements)
    {
        return new User(elements);
    }

    public static If If(bool value, [CallerArgumentExpression("value")] string? variableName = default)
    {
        return new If(value, variableName);
    }

    public static If If(Expression<Func<bool>> condition)
    {
        if (condition.Body is BinaryExpression { NodeType: ExpressionType.Equal } binaryExpression)
        {
            var left = binaryExpression.Left;
            var right = binaryExpression.Right;
            if (left is MemberExpression leftMemberExpression && right is ConstantExpression rightConstantExpression)
            {
                return DSL.If.Equals(leftMemberExpression.Member.Name, rightConstantExpression.Value.ToString());
            }

            if (right is MemberExpression rightMemberExpression && left is ConstantExpression leftConstantExpression)
            {
                return DSL.If.Equals(rightMemberExpression.Member.Name, leftConstantExpression.Value.ToString());
            }
        }
        throw new NotImplementedException();
    }
    
    public static BaseElement Generate(string? variable, [CallerArgumentExpression("variable")] string? variableName = default)
    {
        return new Generate(variable, variableName);
    }
    
    public static BaseElement Var(string? variable, [CallerArgumentExpression("variable")] string? variableName = default)
    {
        return new Variable(variableName!);
    }
    
    public static BaseElement Text(params NonFormattableString[] @string)
    {
        return new Text(string.Join("", @string.Select(s => s.ToString())));
    }
    
    public static BaseElement Text(FormattableString text, [CallerArgumentExpression("text")] string expression = "")
    {
        var format = text.Format;
        var arguments = text.GetArguments();
        var argumentNames = Regex.Matches(expression, @"{([^}]+)}").Select(m => m.Groups[1].Value).ToArray();
        
        var newFormat = Regex.Replace(format, @"{(\d+)}", m => $"{
            arguments[int.Parse(m.Groups[1].Value)] ??
            new Variable(argumentNames[int.Parse(m.Groups[1].Value)]).ToString()
        }");
        
        return new Text(newFormat);
    }
}
