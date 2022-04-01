using System.Linq;

namespace ScrapySharp.Html.Parsing
{
    public class Word
    {
        private readonly string value;
        private readonly int lineNumber;
        private readonly int linePositionEnd;
        private readonly bool isQuoted;
        private readonly bool isWhiteSpace;

        public Word(string value, int lineNumber, int linePositionEnd, bool isQuoted)
        {
            this.value = value;
            this.lineNumber = lineNumber;
            this.linePositionEnd = linePositionEnd;
            this.isQuoted = isQuoted;

            isWhiteSpace = !string.IsNullOrEmpty(value) && value.All(char.IsWhiteSpace);
        }

        public string Value => value;

        public string QuotedValue
        {
            get
            {
                if (IsQuoted)
                {
                    return '"' + value + '"';
                }

                return value;
            }
        }

        public int LineNumber => lineNumber;

        public int LinePositionEnd => linePositionEnd;

        public int LinePositionBegin => linePositionEnd - value.Length;

        public bool IsQuoted => isQuoted;

        public bool IsWhiteSpace => isWhiteSpace;

        public static implicit operator string(Word word)
        {
            return word.Value;
        }

        public static implicit operator char(Word word)
        {
            if (string.IsNullOrEmpty(word.Value))
            {
                return default;
            }

            return word.Value.FirstOrDefault();
        }

    }
}