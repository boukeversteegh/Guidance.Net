using static GuidanceNet.DSL.GuidanceBuilder;
using If = GuidanceNet.DSL.If;

string userInput = null!;
// string response = null!;
//
// var program = Guidance(
//     SystemRole("You are a helpful assistant.", "Decide if the following input is a question, ."),
//     UserRole(Var(question)),
//     AssistantRole(Generate(response)),
//     SystemRole(Text($"The answer to {question} is {response}."))
// );
//
// var template = program.ToString();
// Console.WriteLine(template);

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
    UserRole(Text($"User input: {userInput}")), // Text() supports interpolated strings
    AssistantRole(Generate(userInputType)),
    SystemRole( "You have determined that the user input is a: ", Var(userInputType)),
    If.Equals(userInputType, "command").Then(
        SystemRole(
            "Which of the following commands is the user trying to invoke?\n",
            "/task create\n/task delete\n/calendar list\n/calendar create\n/calendar delete\n"
        ),
        AssistantRole(Generate(command))
    ));

Console.WriteLine(program.ToString());
