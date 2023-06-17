namespace GuidanceNet.DSL;

public class Assistant : RoleBlock
{
    protected override string TagName { get; set; } = "assistant";

    public Assistant()
    {
    }

    public Assistant(params BaseElement[] elements) : base(elements)
    {
    }
}
