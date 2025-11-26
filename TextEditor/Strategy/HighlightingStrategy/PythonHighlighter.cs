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
    public class PythonHighlighter : ISyntaxHighlighter
    {
        private static readonly HashSet<string> Keywords =
            new HashSet<string>
            {
                "def","class","import","from","return",
                "if","elif","else","for","while","in",
                "True","False","None","print","and","or","not"
            };

        public List<Token> Tokenize(string text)
        {
            List<Token> tokens = new List<Token>();
            int i = 0;

            while (i < text.Length)
            {
                char c = text[i];

                if (c == '#')
                {
                    int start = i;
                    while (i < text.Length && text[i] != '\n') i++;

                    tokens.Add(new Token { Start = start, Length = i - start, Type = TokenType.Comment });
                    continue;
                }

                if (c == '"' || c == '\'')
                {
                    char quote = c;
                    int start = i++;
                    while (i < text.Length && text[i] != quote) i++;
                    if (i < text.Length) i++;

                    tokens.Add(new Token { Start = start, Length = i - start, Type = TokenType.StringLiteral });
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
                    while (i < text.Length && char.IsLetterOrDigit(text[i])) i++;

                    string word = text.Substring(start, i - start);

                    tokens.Add(new Token
                    {
                        Start = start,
                        Length = word.Length,
                        Type = Keywords.Contains(word) ? TokenType.Keyword : TokenType.Identifier
                    });

                    continue;
                }

                i++;
            }

            return tokens;
        }
    }
}
