using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Atmosphere.Extensions;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static class SexyParser
    {

        /// <summary>String containing the whitespace characters</summary>
        private const string WHITESPACE = " \t\n\r";
        private const string DELIMITERS = WHITESPACE + "();\"";
        private const string TOKENS = "()\"#';";
        
        /// <summary>String containing all the numeral characters</summary>
        private const string NUMERALS = "+-0123456789eE.";
        
        /// <summary>String containing only those numeral characters which can be at the start of a numeral</summary>
        private const string NUMERALS_START = "+-.0123456789";

        
        /// <summary>String containing only those numeral characters which can be at the start of a numeral</summary>
        private const string IDENTIFIER_START = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ*/<=>!?:$%_&~^";
        private const string IDENTIFIER = IDENTIFIER_START + "0123456789.+-";
        private static readonly Dictionary<char, char> escapeChars = new Dictionary<char, char>
        {
            { '\\', '\\' },
            { '"', '\"' },
            { 'b', '\b' },
            { 'n', '\n' },
            { 'f', '\f' },
            { 'r', '\r' },
            { 't', '\t' },
            { '/', '/' }
        };

        private enum Token
        {
            /// <summary>Represents the lack of a token, or a misplaced token</summary>
            NONE = -1,

            PAREN_OPEN = 0,

            PAREN_CLOSE = 1,

            DOUBLE_QUOTE = 2,

            HASH = 3,

            QUOTE = 4,

            SEMI = 5,

            NUMERAL = 100,

            IDENTIFIER = 200,
        }




        public static bool IsAllWhitespaceAndComments(string sexp)
        {
            return IsAllWhitespaceAndComments(sexp, 0);
        }

        public static bool IsAllWhitespaceAndComments(string sexp, int index)
        {
            bool allwc = true;
                        
            // Must skip whitespace first; NextToken will skip whitespace
            //    but will also throw if the end of the sexp is reached
            SkipWhitespace(sexp, ref index);

            while (index < sexp.Length)
            {
                Token next = NextToken(sexp, ref index);
             
                if (next == Token.SEMI)
                {
                    SkipComment(sexp, ref index);

                    // Must skip whitespace since SkipComment doesn't consume
                    //     the ending newline
                    SkipWhitespace(sexp, ref index);
                }
                else
                {
                    allwc = false;
                    break;
                }
            }

            return allwc;
        }


                
        
        public static ISExp Parse(string sexp)
        {
            int index = 0;
            
            return Parse(sexp, ref index);
        }
        
        public static ISExp Parse(string sexp, ref int index)
        {
            SkipWhitespace(sexp, ref index);

            Token token = PeekToken(sexp, index);

            switch (token)
            {
                case Token.PAREN_OPEN:
                    return ParseList(sexp, ref index);
                    
                case Token.DOUBLE_QUOTE:
                    return ParseString(sexp, ref index);    
                    
                case Token.NUMERAL:
                    return ParseNumber(sexp, ref index);
                    
                case Token.HASH:
                    return ParseHashRepresentation(sexp, ref index);
                    
                case Token.QUOTE:
                    return ParseQuote(sexp, ref index);

                case Token.SEMI:
                    SkipComment(sexp, ref index);
                    return Parse(sexp, ref index);
                    
                case Token.IDENTIFIER:
                    return ParseSymbol(sexp, ref index);
                    
                case Token.NONE:
                default:
                    throw new SexyParserException("Invalid token at index {0}! Char: {1}", index, sexp[index]);
            }
        }

        private static Token PeekToken(string sexp, int index)
        {
            int temp = index;

            return NextToken(sexp, ref temp);
        }

        private static Token NextToken(string sexp, ref int index)
        {
            SkipWhitespace(sexp, ref index);

            if (index >= sexp.Length)
                throw new SexyParserException("Unexpected end of file at index {0}", index);

            char c = sexp[index++];

            int tokenIndex = TOKENS.IndexOf(c);

            if (tokenIndex == -1)
            {
                tokenIndex = NUMERALS_START.IndexOf(c);

                if (tokenIndex != -1)
                {
                    tokenIndex = (int)Token.NUMERAL;
                }
                else
                {
                    tokenIndex = IDENTIFIER_START.IndexOf(c);

                    if (tokenIndex != -1)
                    {
                        tokenIndex = (int)Token.IDENTIFIER;
                    }
                }
            }

            return (Token)tokenIndex;
        }



        #region Parse List

        private static ISExp ParseList(string sexp, ref int index)
        {
            if (PeekToken(sexp, index) != Token.PAREN_OPEN)
            {
                throw new SexyParserException("List expected at index {0}.", index);
            }

            // skip open paren
            NextToken(sexp, ref index);

            List<ISExp> elements = new List<ISExp>();

            while (index < sexp.Length)
            {
                if (PeekToken(sexp, index) == Token.PAREN_CLOSE)
                {
                    // Skip close paren
                    NextToken(sexp, ref index);

                    Pair pair = Pair.List(elements.ToArray());

                    return pair;
                }

                ISExp element = Parse(sexp, ref index);

                elements.Add(element);
            }
            
            throw new SexyParserException("Unexpected end of list representation at index {0}.", index);
        }

        #endregion Parse List





        private static ISExp ParseQuote(string sexp, ref int index)
        {
            if (PeekToken(sexp, index) != Token.QUOTE)
            {
                throw new SexyParserException("Quoted expression expected at index {0}.", index);
            }

            // consume '
            NextToken(sexp, ref index);

            ISExp quoted = Parse(sexp, ref index);

            Pair pair = Pair.List(Atom.KeywordQuote, quoted);

            return pair;
        }



        
        /// <summary>
        /// Parses a SEXP number starting at <paramref name="index" /> and 
        /// advances <paramref name="index" /> to the end of the number representation.
        /// </summary>
        /// <param name="sexp">string to parse</param>
        /// <param name="index">starting index at which to parse</param>
        /// <returns>A <see cref="long"/> or a <see cref="double" />, depending on the representation</returns>
        /// <exception cref="SexpParserException">
        /// Thrown when <paramref name="buffer" /> contains an invalid number at <paramref name="index" />,
        /// or the end of the buffer is reached.
        /// </exception>
        private static ISExp ParseNumber(string sexp, ref int index)
        {            
            if (PeekToken(sexp, index) != Token.NUMERAL)
                throw new SexyParserException("Numeral start character expected at index {0}.", index);

            StringBuilder sb = new StringBuilder();
            
            sb.Append(sexp[index]);                
            
            while (++index < sexp.Length)
            {                
                char c = sexp[index];
                
                if (NUMERALS.IndexOf(c) == -1) break;
                
                sb.Append(c);
            }

            string value = sb.ToString();

            Atom atom = null;
            
            if (value.EqualsAny("+", "-", "...", "."))
            {
                atom = Atom.CreateSymbol(value);   
            }
            else
            {   
                // TODO: overflow on 32-bit integers?
            
                Number number = Number.Parse(sb.ToString());

                atom = Atom.CreateNumber(number);
            }

            return atom;
        }
        
        /// <summary>
        /// Parses a SEXP string starting at <paramref name="index" /> and 
        /// advances <paramref name="index" /> to after the terminating quote.
        /// </summary>
        /// <param name="sexp">string to parse</param>
        /// <param name="index">starting index at which to parse</param>
        /// <returns>The string represented by the working part of <paramref name="sexp" /></returns>
        /// <exception cref="SexpParserException">
        /// Thrown when <paramref name="sexp" /> contains an invalid string at 
        /// <paramref name="index"/>, or the end of the buffer is reached.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Thrown when <paramref name="sexp" /> contains a
        /// string with a '\u' escape sequence.
        /// </exception>
        /// <exception cref="FormatException">
        /// Thrown when <paramref name="sexp" /> contains an unrecognized escape sequence.
        /// </exception>
        private static ISExp ParseString(string sexp, ref int index)
        {
            if (PeekToken(sexp, index) != Token.DOUBLE_QUOTE)
                throw new SexyParserException("String expected at index {0}", index);
            
            StringBuilder sb = new StringBuilder();

            // skip quote character
            index++;
            
            while (index < sexp.Length)
            {
                char c = sexp[index++];
                
                if (c == '\\') // escape character
                {
                    if (index >= sexp.Length)
                        break;
                    
                    char e = sexp[index++];
                    
                    if (escapeChars.ContainsKey(e))
                    {
                        sb.Append(escapeChars[e]);
                    }
                    else
                    {
                        if (e == 'u')
                        {
                            // TODO: implement '\u' handling
                            throw new NotSupportedException(String.Format("\\u string escape sequence found at index {0}! Sequence not supported.", index - 2));
                        }
                        else
                        {
                            throw new FormatException(String.Format("Unrecognized escape sequence at index {0}.", index - 2));
                        }
                    }
                }
                else if (c == '"')
                {
                    string value = sb.ToString();
                    
                    return Atom.CreateString(value);
                }
                else
                {
                    sb.Append(c);
                }
            }
            
            // We got to the end of the buffer but the string was not closed
            throw new SexyParserException("Unexpected end of string representation at index {0}.", index);
        }
        
        /// <summary>
        /// Parses a SEXP symbol starting at <paramref name="index" /> and 
        /// advances <paramref name="index" /> to after the terminating quote.
        /// </summary>
        /// <param name="sexp">string to parse</param>
        /// <param name="index">starting index at which to parse</param>
        /// <returns>The symbol represented by the working part of <paramref name="sexp" /></returns>
        /// <exception cref="SexpParserException">
        /// Thrown when <paramref name="sexp" /> contains an invalid string at 
        /// <paramref name="index"/>, or the end of the buffer is reached.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Thrown when <paramref name="sexp" /> contains a
        /// string with a '\u' escape sequence.
        /// </exception>
        /// <exception cref="FormatException">
        /// Thrown when <paramref name="sexp" /> contains an unrecognized escape sequence.
        /// </exception>
        private static ISExp ParseSymbol(string sexp, ref int index)
        {
            if (PeekToken(sexp, index) != Token.IDENTIFIER)
                throw new SexyParserException("Symbol expected at index {0}", index);
            
            StringBuilder sb = new StringBuilder();
            
            sb.Append(sexp[index]);
            
            while (++index < sexp.Length)
            {
                char c = sexp[index];
                
                if (IDENTIFIER.IndexOf(c) == -1)
                {
                    break;
                }
                else
                {
                    sb.Append(c);
                }
            }
            
            string value = sb.ToString();
            
            return Atom.CreateSymbol(value);
        }

        private static ISExp ParseHashRepresentation(string sexp, ref int index)
        {
            if (PeekToken(sexp, index) != Token.HASH)
                throw new SexyParserException("Char or boolean expected at index {0}", index);

            int start = index;

            // Skip hash symbol and take following char
            char c = sexp[++index];

            switch (c)
            {
                case 'f':
                case 't':

                    // Skip 'f'/'t'
                    index++;

                    // NOTE: no need for delimiter after these two constants (not an error).

                    return Atom.CreateBoolean(c == 't');

                case '\\':

                    // Skip past '\' and take following char
                    char character = sexp[++index];

                    if (character == 'u')
                    {
                        // TODO: implement '\u' handling
                        throw new NotSupportedException(String.Format("\\u character escape sequence found at index {0}! Sequence not supported.", start));
                    }

                    // Skip char
                    index++;
                    
                    // Verify that either we've reached the end of the string
                    //    or the next character is a delimiter
                    if (index != sexp.Length && DELIMITERS.IndexOf(sexp[index]) == -1)
                    {
                        throw new SexyParserException("Bad character at index {0}", start);
                    }

                    return Atom.CreateChar(character);

                default:

                    // TODO: handle numbers that start this way (#e, #i, #b, #o, #d, #x)

                    // TODO: handle vectors?
                    
                    throw new SexyParserException("Bad sequence at index {0}", start);

            }
        }
        
        /// <summary>
        /// Starting at <paramref name="index"/>, advance <paramref name="index" /> to the next 
        /// non-whitespace character in <paramref name="sexp" />.
        /// </summary>
        /// <param name="sexp">string to parse</param>
        /// <param name="index">starting index at which to parse</param>      
        private static void SkipWhitespace(string sexp, ref int index)
        {
            for (; index < sexp.Length; index++)
            {
                if (WHITESPACE.IndexOf(sexp[index]) == -1)
                {
                    break;
                }
            }
        }
        
        /// <summary>
        /// Starting at <paramref name="index"/>, advance <paramref name="index" /> to
        /// the next newline character.
        /// </summary>
        /// <param name="sexp">string to parse</param>
        /// <param name="index">starting index at which to parse</param>      
        private static void SkipComment(string sexp, ref int index)
        {
            for (; index < sexp.Length; index++)
            {
                if (Environment.NewLine.IndexOf(sexp[index]) >= 0)
                {
                    break;
                }
            }
        }
    }
}

