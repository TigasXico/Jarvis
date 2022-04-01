using System.Globalization;
using System.Text;

using ScrapySharp.Extensions;

namespace ScrapySharp.Html.Parsing
{
    public class CodeReader
    {
        private readonly string sourceCode;
        private readonly StringBuilder buffer;
        private int position;
        private CodeReadingContext context;

        private int lineNumber = 1;
        private int linePosition = 1;

        public CodeReader(string sourceCode)
        {
            if (sourceCode.EndsWith("\n"))
            {
                this.sourceCode = sourceCode;
            }
            else
            {
                this.sourceCode = sourceCode + "\n";
            }

            buffer = new StringBuilder();
            context = CodeReadingContext.None;
        }

        public int MaxWordCount => sourceCode.Length;

        public Word ReadWord()
        {
            buffer.Remove(0, buffer.Length);
            char c = ReadChar();

            //while (char.IsWhiteSpace(c))
            //    c = ReadChar();

            if (char.IsWhiteSpace(c))
            {
                return new Word(c.ToString(CultureInfo.InvariantCulture), lineNumber, linePosition, false);
            }

            if (context != CodeReadingContext.InQuotes && (c == Tokens.Quote || c == Tokens.SimpleQuote))
            {
                context = CodeReadingContext.InQuotes;
                return ReadQuotedString(c);
            }

            buffer.Append(c);

            bool letterOrDigit = IsLetterOrDigit(c);

            while (IsLetterOrDigit(GetNextChar()) == letterOrDigit && !char.IsWhiteSpace(GetNextChar()) && !GetNextChar().IsToken())
            {
                c = ReadChar();
                if (c == Tokens.Quote)
                {
                    position--;
                    break;
                }

                buffer.Append(c);

                //if (c.IsToken() && GetNextChar().IsToken())
                //    break;
            }

            return new Word(buffer.ToString(), lineNumber, linePosition, false);
        }

        private Word ReadQuotedString(char quoteChar)
        {
            char c = ReadChar();
            
            while (!End && context == CodeReadingContext.InQuotes)
            {
                if (c == quoteChar)
                {
                    break;
                }

                char nextChar = GetNextChar();
                if (nextChar == Tokens.TagBegin || nextChar == Tokens.TagEnd)
                {
                    break;
                }

                buffer.Append(c);

                if (c == Tokens.TagBegin || c == Tokens.TagEnd)
                {
                    break;
                }

                c = ReadChar();
            }

            context = CodeReadingContext.None;

            return new Word(buffer.ToString(), lineNumber, linePosition, true);
        }


        public char GetNextChar()
        {
            if (position >= sourceCode.Length)
            {
                return (char)0;
            }

            return sourceCode[position];
        }

        public char GetPreviousChar()
        {
            if (position <= 1)
            {
                return (char)0;
            }

            return sourceCode[position - 2];
        }

        public char ReadChar()
        {
            if (End)
            {
                return (char)0;
            }

            char c = sourceCode[position++];
            linePosition++;

            if (c == '\n')
            {
                lineNumber++;
                linePosition = 1;
            }

            return c;
        }

        public bool End => position >= sourceCode.Length;

        public int LineNumber => lineNumber;

        public int LinePosition => linePosition;


        public bool IsLetterOrDigit(char c)
        {
            return char.IsLetterOrDigit(c) || c == '-' || c == '_'
                || c == ':' || c == ';' || c == '+';
        }
    }
}