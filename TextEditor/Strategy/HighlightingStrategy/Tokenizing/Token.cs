using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Strategy.HighlightingStrategy.Tokenizing
{
    public class Token
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public TokenType Type { get; set; }
    }
}
