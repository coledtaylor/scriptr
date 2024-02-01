namespace Scriptr;

class Parser(List<Token> tokens)
{
    private class ParseError(Token token, string message) : Exception { }

    private readonly List<Token> _tokens = tokens;
    private int _current = 0;

    public Expr Parse()
    {
        try
        {
            return Expression();
        }
        catch (Exception)
        {
            return null;
        }
    }

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
        Expr expression = Term();

        while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
        {

            Token operative = Previous();
            Expr right = Term();
            expression = new Expr.Binary(expression, operative, right);
        }

        return expression;
    }

    private Expr Term()
    {
        Expr expression = Factor();

        while (Match(TokenType.PLUS, TokenType.MINUS))
        {
            Token operative = Previous();
            Expr right = Factor();
            expression = new Expr.Binary(expression, operative, right);
        }

        return expression;
    }

    private Expr Factor()
    {
        Expr expression = Unary();

        while (Match(TokenType.SLASH, TokenType.STAR))
        {
            Token operative = Previous();
            Expr right = Unary();
            expression = new Expr.Binary(expression, operative, right);
        }

        return expression;
    }

    private Expr Unary()
    {
        if (Match(TokenType.MINUS, TokenType.MINUS))
        {
            Token operative = Previous();
            Expr right = Primary();
            return new Expr.Unary(operative, right);
        }

        return Primary();
    }

    private Expr Primary()
    {
        if (Match(TokenType.FALSE)) return new Expr.Literal(false);
        if (Match(TokenType.TRUE)) return new Expr.Literal(true);
        if (Match(TokenType.NIL)) return new Expr.Literal(null);

        if (Match(TokenType.NUMBER, TokenType.STRING))
        {
            return new Expr.Literal(Previous().Literal);
        }

        if (Match(TokenType.LEFT_PAREN))
        {
            Expr expression = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
            return new Expr.Grouping(expression);
        }

        throw new ParseError(Peek(), "Expected Expression");
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

    private Token Consume(TokenType type, string message)
    {
        return Check(type) ? Advance() : throw new ParseError(Peek(), message);
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