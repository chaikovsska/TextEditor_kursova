using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Strategy.HighlightingStrategy.Tokenizing
{
    public enum TokenType
    {
        Text,
        Keyword,
        Comment,
        StringLiteral,
        Number,
        Identifier,
        Operator
    }
}
