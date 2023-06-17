namespace GuidanceNet.DSL;

public class NonFormattableString
{
    private NonFormattableString(string s)
    {
        String = s;
    }

    private string String { get; }
    
    public static implicit operator NonFormattableString(string s)
    {
        return new NonFormattableString(s);
    }

    public static implicit operator NonFormattableString(FormattableString fs)
    {
        throw new InvalidOperationException(
            "Missing FormattableString overload of method taking this type as argument");
    }

    public override string ToString()
    {
        return String;
    }
}
