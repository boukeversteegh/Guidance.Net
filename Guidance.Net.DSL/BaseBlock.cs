using System.Collections;
using System.Linq.Expressions;

namespace GuidanceNet.DSL;

public abstract class BaseBlock
    : BaseElement, IEnumerable<BaseElement>
{
    protected abstract string TagName { get; set; }
    private ICollection<BaseElement> Elements { get; } = new List<BaseElement>();

    protected BaseBlock(params BaseElement[] elements)
    {
        foreach (var element in elements) {
            Add(element);
        }
    }

    public IEnumerator<BaseElement> GetEnumerator()
    {
        return Elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(BaseElement baseElement)
    {
        Elements.Add(baseElement);
    }

    public void Add(string text)
    {
        Elements.Add(new Text(text));
    }

    public void Add(Expression<Func<string?>> getter)
    {
        Elements.Add(new Variable(getter));
    }
    
    

    public override string ToString()
    {
        var body = Body;

        return $@"{BlockOpen(body)}{body}{BlockClose(body)}";
    }
    
    protected string Body => string.Join("", Elements);

    protected string BlockOpen(string body, params string[] arguments)
    {
        var optionalArguments = arguments.Length > 0 ? " " + string.Join(" ", arguments) : "";
        
        var allowPrettyPrint = body == body.TrimStart();
        var optionalTrimMarker = allowPrettyPrint ? @"~" : "";
        var optionalNewline = allowPrettyPrint ? "\n" : "";
        
        return $@"{{{{#{TagName}{optionalArguments}{optionalTrimMarker}}}}}{optionalNewline}";
    }


    protected string BlockClose(string body)
    {
        var allowPrettyPrint = body == body.TrimEnd();
        var optionalTrimMarker = allowPrettyPrint ? @"~" : "";
        var optionalNewline = allowPrettyPrint ? "\n" : "";
        
        return $@"{optionalNewline}{{{{{optionalTrimMarker}/{TagName}}}}}";
    }
}
