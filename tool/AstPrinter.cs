using System.Text;

using Lox;

namespace Tool;

class AstPrinter : Expr.IVisitor<string>
{

    public string Print(Expr expression)
    {
        return expression.Accept(this);
    }

    public string VisitBinaryExpr(Expr.Binary expr)
    {
        return Parenthesize(expr.Operative.Lexeme, expr.Left, expr.Right);
    }

    public string VisitGroupingExpr(Expr.Grouping expr)
    {
        return Parenthesize("group", expr.Expression);
    }

    public string VisitLiteralExpr(Expr.Literal expr)
    {
        return expr.Value == null ? "nil" : expr.Value.ToString();
    }

    public string VisitUnaryExpr(Expr.Unary expr)
    {
        return Parenthesize(expr.Operative.Lexeme, expr.Right);
    }

    public string Parenthesize(string name, params Expr[] exprs)
    {
        StringBuilder builder = new();

        builder.Append('(').Append(name);

        foreach (Expr expr in exprs)
        {
            builder.Append(' ');
            builder.Append(expr.Accept(this));
        }

        builder.Append(')');

        return builder.ToString();
    }
}