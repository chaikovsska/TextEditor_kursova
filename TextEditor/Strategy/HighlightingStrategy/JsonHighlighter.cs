using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextEditor.Strategy.HighlightingStrategy.Tokenizing;

namespace TextEditor.Strategy.HighlightingStrategy
{
    public class JsonHighlighter : ISyntaxHighlighter
    {
        public List<Token> Tokenize(string text)
        {
            List<Token> tokens = new List<Token>();
            int i = 0;

            while (i < text.Length)
            {
                char c = text[i];

                if (c == '"')
                {
                    int start = i++;
                    while (i < text.Length && text[i] != '"') i++;
                    if (i < text.Length) i++;

                    int p = i;
                    while (p < text.Length && char.IsWhiteSpace(text[p])) p++;

                    bool isKey = (p < text.Length && text[p] == ':');

                    tokens.Add(new Token
                    {
                        Start = start,
                        Length = i - start,
                        Type = isKey ? TokenType.Keyword : TokenType.StringLiteral
                    });

                    continue;
                }

                if (char.IsDigit(c))
                {
                    int start = i;
                    while (i < text.Length && (char.IsDigit(text[i]) || text[i] == '.')) i++;

                    tokens.Add(new Token { Start = start, Length = i - start, Type = TokenType.Number });
                    continue;
                }

                if (char.IsLetter(c))
                {
                    int start = i;
                    while (i < text.Length && char.IsLetter(text[i])) i++;

                    string word = text.Substring(start, i - start);

                    if (word == "true" || word == "false" || word == "null")
                    {
                        tokens.Add(new Token
                        {
                            Start = start,
                            Length = word.Length,
                            Type = TokenType.Identifier
                        });
                    }

                    continue;
                }

                i++;
            }

            return tokens;
        }
    }
}
