using System.Collections;

namespace GuidanceNet.DSL;

public class GuidanceProgram : IEnumerable<BaseElement>
{
    private ICollection<BaseElement> Elements { get; } = new List<BaseElement>();

    public void Add(BaseElement child)
    {
        Elements.Add(child);
    }

    IEnumerator<BaseElement> IEnumerable<BaseElement>.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return string.Join("\n", Elements);
    }
}