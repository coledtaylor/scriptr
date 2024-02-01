namespace Scriptr;

abstract class Expr
{

    public interface IVisitor<T>
    {
        T VisitBinaryExpr(Binary binary);
        T VisitGroupingExpr(Grouping grouping);
        T VisitLiteralExpr(Literal literal);
        T VisitUnaryExpr(Unary unary);
    }

    public class Binary(Expr left, Token operative, Expr right) : Expr
    {
        public readonly Expr Left = left;
        public readonly Token Operative = operative;
        public readonly Expr Right = right;

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }
    };

    public class Grouping(Expr expression) : Expr
    {
        public readonly Expr Expression = expression;

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitGroupingExpr(this);
        }
    };

    public class Literal(object value) : Expr
    {
        public readonly object Value = value;

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }
    };

    public class Unary(Token operative, Expr right) : Expr
    {
        public readonly Token Operative = operative;
        public readonly Expr Right = right;

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    };

    public abstract T Accept<T>(IVisitor<T> visitor);

}
