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

    public static If If(bool variable, [CallerArgumentExpression("variable")] string? variableName = default)
    {
        return new If(variable, variableName);
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
