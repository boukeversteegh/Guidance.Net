# Guidance.Net

**Guidance.Net** is a library that makes it easy and fun to write and run [Guidance](https://github.com/microsoft/guidance)programs from C# and .Net.

## Guidance.Net DSL

Guidance.Net.DSL is a powerful and type-safe Domain Specific Language (DSL) library for C# developers that simplifies the process of creating guidance programs, which are templates for chatbots and other generative language models. The library provides a collection of building blocks, allowing you to create structured and maintainable interactive dialogs.

Features:

- Concise syntax: shorter and more readable than handlebars guidance templates
- Automatic pretty-printing
- Compile-checked variable references

### Basic Example

```csharp
using static GuidanceNet.DSL.GuidanceBuilder;

string question = null!;
string response = null!;

var program = Guidance(
    SystemRole("You are a helpful assistant."),
    UserRole(Var(question)),
    AssistantRole(Generate(response)),
    SystemRole(Text($"The answer to {question} is {response}."))
);

var template = program.ToString();
Console.WriteLine(template);
```

Generated guidance template:

```handlebars
{{#system~}}
You are a helpful assistant.
{{~/system}}
{{#user~}}
{{question}}
{{~/user}}
{{#assistant~}}
{{gen 'response'}}
{{~/assistant}}
{{#system~}}
The answer to {{question}} is {{response}}.
{{~/system}}
```

### Advanced example

```csharp
var isConversationMode = false;
string userInputType = null!;
string command = null!;
var program = Guidance(
    SystemRole(
        "You are a helpful assistant. ",
        If(isConversationMode)
            .Then(@"Conversation mode is enabled. This enables TTS. 
Use pronounceable words only. Please insert a <break> tag after each sentence, phrase or line.")
            .Else("Conversation mode is disabled.")
    ),
    AssistantRole("Hi, I'm your assistant. Ask me anything."),
    SystemRole(
        "Determine whether the following user input is a question, statement or command. Return a single word, being either 'question', 'statement' or 'command'."),
    UserRole(Text($"User input: {userInput}")),
    AssistantRole(Generate(userInputType)),
    SystemRole( "You have determined that the user input is a: ", Var(userInputType)),
    If.Equals(userInputType, "command").Then(
        SystemRole(
            "Which of the following commands is the user trying to invoke?\n",
            "/task create\n/task delete\n/calendar list\n/calendar create\n/calendar delete\n"
        ),
        AssistantRole(Generate(command))
    ));
```

```handlebars
{{#system~}}
You are a helpful assistant. {{#if isConversationMode~}}
Conversation mode is enabled. This enables TTS.
Use pronounceable words only. Please insert a <break> tag after each sentence, p
hrase or line.{{else}}Conversation mode is disabled.
{{~/if}}
{{~/system}}
{{#assistant~}}
Hi, I'm your assistant. Ask me anything.
{{~/assistant}}
{{#system~}}
Determine whether the following user input is a question, statement or command.
Return a single word, being either 'question', 'statement' or 'command'.
{{~/system}}
{{#user~}}
User input{{userInput}}
{{~/user}}
{{#assistant~}}
{{gen 'userInputType'}}
{{~/assistant}}
{{#system~}}
You have determined that the user input is a: {{userInputType}}
{{~/system}}
{{#if (== userInputType "command")~}}
{{#system~}}
Which of the following commands is the user trying to invoke?
/task create
/task delete
/calendar list
/calendar create
/calendar delete
{{/system}}{{#assistant~}}
{{gen 'command'}}
{{~/assistant}}
{{~/if}}
```

### Syntax

| Guidance.NET         | Guidance                    | Description                                                                                              |
| -------------------- | --------------------------- | -------------------------------------------------------------------------------------------------------- |
| `SystemRole(...)`    | `{{#system}}...{{/system}}` | Add a system message to the conversation.                                                                |
| `UserRole(...)`      | `{{#user}}...{{/user}}`    | Add a user input to the conversation.                                                                    |
| `AssistantRole(...)` | `{{#assistant}}...{{/assistant}}` | Add the assistant's response to the conversation.                                                     |
| "hello" | `hello` | Static text within a message. |
| `Text("Hello", "!")` | `hello!` | Static text inside a message.        |
| `string name = null;`<br />`Text($"User name: {name}")` | `User name: {{name}}` | Text with dynamic content. If the variable has no value, a placeholder variable is generated, which will be filled in when executing the guidance program. |
| `string name = "Jay";`<br />`Text($"User name: {name}")` | `User name: Jay` | Text with dynamic content. If the variable has a value, it will be inserted immediately. |
| `Var(name)`        | `{{name}}`              | Access and include variables and their values in the conversation.                             |
| `Generate(outputVar)` | `{{gen 'outputVar'}}` | Instruct the language model to generate text, and store inside the specified variable. |
| `If(condition).Then(...)` | `{{#if condition}}...{{/if}}` | Add conditional logic to the conversation with specific blocks to be included if a condition is met. Can be defined inside or outside of messages. |
| `If(condition).Then(...).Else(...)` | {{#if condition}}...{{else}}...{{/if}} | Add an else block to an if block. |
| `If.Equals(example, "value").Then(...)` | `{{#if (== example "value")}}...{{/if}}` | Add conditional logic to the conversation with specific blocks to be included if two values are equal. |
