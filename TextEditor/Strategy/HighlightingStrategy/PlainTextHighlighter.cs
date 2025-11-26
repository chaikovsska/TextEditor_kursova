using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextEditor.Strategy.HighlightingStrategy.Tokenizing;

namespace TextEditor.Strategy.HighlightingStrategy
{
    public class PlainTextHighlighter : ISyntaxHighlighter
    {
        public List<Token> Tokenize(string text)
        {
            return new List<Token>(); 
        }
    }
}
