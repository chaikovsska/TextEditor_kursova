using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor.Strategy.HighlightingStrategy.Tokenizing
{
    public static class TokenRenderer
    {
        public static void Paint(RichTextBox editor, List<Token> tokens, int visibleStart, int visibleEnd)
        {
            if (tokens == null || tokens.Count == 0) return;


            foreach (var t in tokens)
            {
                if (t.Start + t.Length < visibleStart) continue;
                if (t.Start > visibleEnd) break;

                if (t.Start < 0 || t.Start + t.Length > editor.TextLength) continue;

                editor.Select(t.Start, t.Length);
                editor.SelectionColor = ColorFor(t.Type);
            }
        }

        private static Color ColorFor(TokenType type)
        {
            switch (type)
            {
                case TokenType.Keyword: return Color.Blue;
                case TokenType.Comment: return Color.Green;
                case TokenType.StringLiteral: return Color.Brown;
                case TokenType.Number: return Color.Purple;
                default: return Color.Black;
            }
        }
    }
}
