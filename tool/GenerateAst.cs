namespace Tool;

class GenerateAst
{
    public static void DefineAst(string outputDir, string baseName, List<string> types)
    {
        string path = outputDir + "/" + baseName + ".cs";

        using StreamWriter writer = new(path);

        writer.WriteLine("namespace Lox;");
        writer.WriteLine();
        writer.WriteLine("abstract class " + baseName + " {");
        writer.WriteLine();

        DefineVisitor(writer, baseName, types);

        foreach (string type in types)
        {
            string className = type.Split(':')[0].Trim();
            string fields = type.Split(':')[1].Trim();

            DefineType(writer, baseName, className, fields);
        }

        writer.WriteLine("  public abstract T Accept<T>(IVisitor<T> visitor);");
        writer.WriteLine();

        writer.WriteLine("}");
        writer.Close();
    }

    private static void DefineType(StreamWriter writer, string baseName, string className, string fieldList)
    {
        writer.WriteLine("  public class " + className + "(" + fieldList + ")" + " : " + baseName + " {");

        string[] fields = fieldList.Split(", ");

        foreach (string field in fields)
        {
            string type = field.Split(" ")[0];
            string name = field.Split(" ")[1];

            writer.WriteLine("      public readonly " + type + " " + name[0].ToString().ToUpper() + name[1..] + " = " + name + ";");
        }

        writer.WriteLine();

        writer.WriteLine("      public override T Accept<T>(IVisitor<T> visitor) {");
        writer.WriteLine("          return visitor.Visit" + className + baseName + "(this);");
        writer.WriteLine("      }");

        writer.WriteLine("  };");
        writer.WriteLine();
    }

    private static void DefineVisitor(StreamWriter writer, string baseName, List<string> types)
    {
        writer.WriteLine("  public interface IVisitor<T> {");

        foreach (string type in types)
        {
            string className = type.Split(':')[0].Trim();

            writer.WriteLine("      T Visit" + className + baseName + "(" + className + " " + className.ToLower() + ");");
        }

        writer.WriteLine("  }");
        writer.WriteLine();
    }
}