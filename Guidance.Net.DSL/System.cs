namespace GuidanceNet.DSL;

public class System : RoleBlock
{
    protected override string TagName { get; set; } = "system";

    public System()
    {
    }

    public System(params BaseElement[] elements) : base(elements)
    {
    }
}
