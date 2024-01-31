
namespace Lox;

class Parser(Token[] tokens)
{
    private readonly Token[] _tokens = tokens;
    private int _current = 0;

    private Expr Expression()
    {
        return Equality();
    }

    private Expr Equality()
    {
        Expr expression = Comparison();

        while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
        {
            Token operative = Previous();
            Expr right = Comparison();
            expression = new Expr.Binary(expression, operative, right);
        }

        return expression;
    }

    private Expr Comparison()
    {
        return Term();
    }

    private Expr Term()
    {
        return Factor();
    }

    private Expr Factor()
    {
        return Unary();
    }

    private Expr Unary()
    {
        return Primary();
    }

    private Expr Primary()
    {
        throw new NotImplementedException();
    }

    private bool Match(params TokenType[] types)
    {
        foreach (TokenType type in types)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }

        return false;
    }

    private Token Advance()
    {
        if (!IsAtEnd()) _current++;
        return Previous();
    }

    private Token Previous()
    {
        return _tokens[_current - 1];
    }

    private bool Check(TokenType type)
    {
        return !IsAtEnd() && Peek().Type == type;
    }

    private Token Peek()
    {
        return _tokens[_current];
    }

    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EOF;
    }
}