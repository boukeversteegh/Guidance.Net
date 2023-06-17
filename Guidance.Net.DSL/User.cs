namespace GuidanceNet.DSL;

public class User : RoleBlock
{
    protected override string TagName { get; set; } = "user";

    public User()
    {
            
    }

    public User(params BaseElement[] elements) : base(elements)
    {
    }
}
