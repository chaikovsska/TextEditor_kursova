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
    public class XmlHighlighter : ISyntaxHighlighter
    {
        public List<Token> Tokenize(string text)
        {
            List<Token> tokens = new List<Token>();
            int i = 0;

            while (i < text.Length)
            {
                char c = text[i];

                if (c == '<')
                {
                    int start = i++;
                    while (i < text.Length && text[i] != '>') i++;
                    if (i < text.Length) i++;

                    tokens.Add(new Token { Start = start, Length = i - start, Type = TokenType.Keyword });
                    continue;
                }

                if (char.IsLetter(c))
                {
                    int start = i;
                    while (i < text.Length && char.IsLetterOrDigit(text[i])) i++;

                    if (i < text.Length && text[i] == '=')
                    {
                        tokens.Add(new Token { Start = start, Length = i - start, Type = TokenType.Identifier });
                        continue;
                    }
                }

                if (c == '"')
                {
                    int start = i++;
                    while (i < text.Length && text[i] != '"') i++;
                    if (i < text.Length) i++;

                    tokens.Add(new Token { Start = start, Length = i - start, Type = TokenType.StringLiteral });
                    continue;
                }

                if (c == '<' && i + 3 < text.Length &&
                    text[i + 1] == '!' && text[i + 2] == '-' && text[i + 3] == '-')
                {
                    int start = i;
                    i += 4;
                    while (i + 2 < text.Length &&
                           !(text[i] == '-' && text[i + 1] == '-' && text[i + 2] == '>'))
                    {
                        i++;
                    }
                    if (i + 2 < text.Length) i += 3;

                    tokens.Add(new Token { Start = start, Length = i - start, Type = TokenType.Comment });
                    continue;
                }

                i++;
            }

            return tokens;
        }
    }
}
