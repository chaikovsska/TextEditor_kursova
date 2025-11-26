using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TextEditor.Strategy.HighlightingStrategy;
using TextEditor.Strategy.HighlightingStrategy.Tokenizing;

namespace TextEditor.Services
{
    public class HighlighterService
    {
        private List<Token> _cachedTokens = new List<Token>();
        private bool _isHighlighting = false;

        public void RecalculateTokens(RichTextBox editor)
        {
            if (editor == null) return;
            try
            {
                var meta = editor.Tag as EditorMetadata;
                string filePath = meta?.FilePath ?? "plain.txt";
                ISyntaxHighlighter strategy = ApplyHighlighting(filePath);

                _cachedTokens = strategy.Tokenize(editor.Text);
            }
            catch
            {
                _cachedTokens = new List<Token>();
            }
        }

        public void ForceRefresh(RichTextBox editor)
        {
            RecalculateTokens(editor);
            HighlightVisibleArea(editor);
        }

        public void HighlightVisibleArea(RichTextBox editor)
        {
            if (editor == null || _isHighlighting) return;
            if (_cachedTokens == null || _cachedTokens.Count == 0) return;

            int originalIndex = editor.SelectionStart;
            int originalLength = editor.SelectionLength;
            Point scrollPos = editor.GetScrollPos();

            _isHighlighting = true;
            try
            {
                editor.BeginUpdate();

                var (start, end) = GetVisibleCharRange(editor);

                TokenRenderer.Paint(editor, _cachedTokens, start, end);
            }
            catch
            {
            }
            finally
            {
                editor.Select(originalIndex, originalLength);
                editor.SelectionColor = editor.ForeColor;

                editor.SetScrollPos(scrollPos);

                editor.EndUpdate();

                _isHighlighting = false;
            }
        }

        private (int start, int end) GetVisibleCharRange(RichTextBox rtb)
        {
            int textLen = rtb.TextLength;
            if (textLen == 0) return (0, 0);
            if (textLen < 50000) return (0, textLen); 

            int startChar = rtb.GetCharIndexFromPosition(new Point(0, 0));
            int startLine = rtb.GetLineFromCharIndex(startChar);

            int endLine = startLine + 200;
            int totalLines = rtb.GetLineFromCharIndex(textLen - 1);

            if (endLine > totalLines) endLine = totalLines;

            int endChar = rtb.GetFirstCharIndexFromLine(endLine);

            int finalEnd = Math.Min(textLen, endChar + 500);

            return (startChar, finalEnd);
        }

        private ISyntaxHighlighter ApplyHighlighting(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return new PlainTextHighlighter();

            string ext = Path.GetExtension(filePath).ToLower();
            switch (ext)
            {
                case ".cs": return new CSharpHighlighter();
                case ".java": return new JavaHighlighter();
                case ".py": return new PythonHighlighter();
                case ".js": case ".ts": return new JavaScriptHighlighter();
                case ".css": return new CssHighlighter();
                case ".json": return new JsonHighlighter();
                case ".xml": case ".html": case ".htm": return new XmlHighlighter();
                default: return new PlainTextHighlighter();
            }
        }
    }
}