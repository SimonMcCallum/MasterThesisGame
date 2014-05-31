namespace Hig.Compiler.LexicalAnalysis
{
    using System;
    using System.Linq;

    public class LexerRule : ILexerRule
    {
        protected string[] _lexems;

        public string Name { get; protected set; }
        public bool IsCaseSensitive { get; set; }

        public LexerRule(string name, bool isCaseSensitive = true, params string[] lexems)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name");
            if (lexems == null)
                throw new ArgumentNullException("lexems");

            Name = name;
            IsCaseSensitive = isCaseSensitive;
            _lexems = lexems;
        }

        public LexerRule(string lexem)
        {
            if (String.IsNullOrEmpty(lexem))
                throw new ArgumentException("lexem");

            Name = lexem;
            _lexems = new[] { lexem };
        }

        public bool Check(string text)
        {
            if (IsCaseSensitive)
                return _lexems.Contains(text);

            for (int i = 0; i < _lexems.Length; i++)
                if (_lexems[i].ToUpper() == text.ToUpper())
                    return true;

            return false;
        }
    }
}
