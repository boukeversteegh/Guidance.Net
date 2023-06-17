namespace GuidanceNet.DSL;

public abstract class BaseElement : IGuidanceElement
{
    public static implicit operator BaseElement(string value)
    {
        return new Text(value);
    }
}