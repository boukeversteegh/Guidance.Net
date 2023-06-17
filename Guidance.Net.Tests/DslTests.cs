using GuidanceNet.DSL;
using JetBrains.Annotations;
using static GuidanceNet.DSL.GuidanceBuilder;


namespace GuidanceNet.Tests;

public class DslTests
{
    [Fact]
    public void Roles()
    {
        Guidance(
                SystemRole("You are a bot."),
                UserRole("What is the capital of France?"),
                AssistantRole("Paris")
            )
            .ExpectTemplate(
                @"{{#system~}}
You are a bot.
{{~/system}}
{{#user~}}
What is the capital of France?
{{~/user}}
{{#assistant~}}
Paris
{{~/assistant}}"
            );
    }

    [Fact]
    public void TextElement()
    {
        var template = Template(@"{{#user~}}
What is the capital of France?
{{~/user}}");
        
        Guidance(UserRole(Text("What is the capital of France?")))
            .ExpectTemplate(template);
        
        Guidance(UserRole("What is the capital of France?"))
            .ExpectTemplate(template);
        
        Guidance(UserRole("What is the capital of ", "France", "?"))
            .ExpectTemplate(template);
    }

    [Fact]
    public void InterpolatedTextTest()
    {
        string? country = null;
        Guidance(UserRole(Text($"What is the capital of {country}?")))
            .ExpectTemplate(@"{{#user~}}
What is the capital of {{country}}?
{{~/user}}");
    }

    [Fact]
    public void Variable()
    {
        const string input = "What is the capital of France?";

        Guidance(UserRole(Var(input)))
            .ExpectTemplate(@"{{#user~}}
{{input}}
{{~/user}}");
    }

    [Fact]
    public void IfBlock()
    {
        const bool systemMessage = true;

        Guidance(
                If(systemMessage).Then(
                    SystemRole("You are a bot.")
                ),
                AssistantRole("Hello!")
            )
            .ExpectTemplate(@"{{#if systemMessage~}}
{{#system~}}
You are a bot.
{{~/system}}
{{~/if}}
{{#assistant~}}
Hello!
{{~/assistant}}");
    }

    [Fact]
    public void GenerateTest()
    {
        const string response = "";
        Guidance(
            UserRole("What is the capital of France?"),
            AssistantRole(Generate(response))
        ).ExpectTemplate(@"{{#user~}}
What is the capital of France?
{{~/user}}
{{#assistant~}}
{{gen 'response'}}
{{~/assistant}}");
    }

    private static string Template([LanguageInjection("handlebars")] string template)
    {
        return template;
    }
}

public static class AssertionExtensions
{
    public static void ExpectTemplate(
        this GuidanceProgram guidanceProgram,
        [LanguageInjection("handlebars")] string template
    )
    {
        Assert.Equal(template, guidanceProgram.ToString());
    }
}
