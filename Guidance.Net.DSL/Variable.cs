using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace GuidanceNet.DSL;

public class Variable : BaseElement
{
    private readonly string name;

    public Variable(string variableName)
    {
        name = variableName;
    }

    public override string ToString()
    {
        return $@"{{{{{name}}}}}";
    }
}