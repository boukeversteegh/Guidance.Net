using System.Runtime.CompilerServices;

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
        return new Variable(variable, variableName);
    }
    
    public static BaseElement Text(string text)
    {
        return new Text(text);
    }
}
