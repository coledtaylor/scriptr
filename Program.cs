using System;
using System.IO;
using System.Text;

using Tool;

namespace Scriptr;

class Program
{
    static bool s_hadError = false;
    static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            if (args[0] == "-g")
            {
                string outputDir = args[1];

                GenerateAst.DefineAst(outputDir, "Expr",
                    [
                        "Binary   : Expr left, Token operative, Expr right",
                        "Grouping : Expr expression",
                        "Literal  : object value",
                        "Unary    : Token operative, Expr right"
                    ]
                );
            }
            else
            {
                RunFile(args[0]);
            }
        }
        else
        {
            RunPrompt();
        }
    }

    private static void RunFile(string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        Run(Encoding.UTF8.GetString(bytes));

        if (s_hadError) Environment.Exit(65);
    }

    private static void RunPrompt()
    {
        for (; ; )
        {
            Console.Write("> ");
            string line = Console.ReadLine();

            Run(line);
        }
    }

    private static void Run(string source)
    {
        Scanner scanner = new(source);
        List<Token> tokens = scanner.ScanTokens();
        Parser parser = new(tokens);
        Expr expression = parser.Parse();


        if (s_hadError) return;

        // foreach (Token token in tokens)
        // {
        //     Console.Write("[" + token.Lexeme + "]");
        // }

        Console.WriteLine(new AstPrinter().Print(expression));
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.Error.WriteLine("[line " + line + "] Error" + where + ": " + message);
        s_hadError = true;
    }
}
