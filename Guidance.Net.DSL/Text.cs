namespace GuidanceNet.DSL;

public class Text : BaseElement
{
    public Text(string content)
    {
        Content = content;
    }

    private string Content { get; set; }

    public override string ToString()
    {
        return Content;
    }
}