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
    public class CssHighlighter : ISyntaxHighlighter
    {
        public List<Token> Tokenize(string text)
        {
            List<Token> tokens = new List<Token>();
            int i = 0;

            while (i < text.Length)
            {
                char c = text[i];

                if (c == '/' && i + 1 < text.Length && text[i + 1] == '*')
                {
                    int start = i;
                    i += 2;
                    while (i + 1 < text.Length && !(text[i] == '*' && text[i + 1] == '/')) i++;
                    if (i + 1 < text.Length) i += 2;

                    tokens.Add(new Token { Start = start, Length = i - start, Type = TokenType.Comment });
                    continue;
                }

                if (char.IsLetter(c))
                {
                    int start = i;
                    while (i < text.Length && (char.IsLetter(text[i]) || text[i] == '-'))
                        i++;

                    if (i < text.Length && text[i] == ':')
                    {
                        tokens.Add(new Token
                        {
                            Start = start,
                            Length = i - start,
                            Type = TokenType.Keyword
                        });
                    }

                    continue;
                }

                if (c == '#')
                {
                    int start = i++;
                    while (i < text.Length && IsHex(text[i])) i++;

                    tokens.Add(new Token { Start = start, Length = i - start, Type = TokenType.Number });
                    continue;
                }

                if (c == '.' || c == '#')
                {
                    int start = i++;
                    while (i < text.Length && (char.IsLetterOrDigit(text[i]) || text[i] == '-' || text[i] == '_'))
                        i++;

                    tokens.Add(new Token { Start = start, Length = i - start, Type = TokenType.Identifier });
                    continue;
                }

                if (char.IsDigit(c))
                {
                    int start = i;
                    while (i < text.Length && char.IsDigit(text[i])) i++;

                    while (i < text.Length && char.IsLetter(text[i])) i++;

                    tokens.Add(new Token { Start = start, Length = i - start, Type = TokenType.Number });
                    continue;
                }

                i++;
            }

            return tokens;
        }

        private bool IsHex(char c)
        {
            return char.IsDigit(c) ||
                   (c >= 'a' && c <= 'f') ||
                   (c >= 'A' && c <= 'F');
        }
    }
}
