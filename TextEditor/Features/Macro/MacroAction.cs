using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Features.Macro
{
    public enum MacroActionType
    {
        InsertText,
        DeleteText,
        MoveCursor,
        ExecuteCommand 
    }

    public class MacroAction
    {
        public MacroActionType Type { get; set; }
        public string Text { get; set; }    
        public int Position { get; set; }  
        public int Length { get; set; }     
        public string CommandName { get; set; } 
    }
}
