using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;

using ScrapySharp.Core;

namespace ScrapySharp.Extensions
{
    public static class CssQueryExtensions
    {
        public static IEnumerable<HtmlNode> CssSelect( this IEnumerable<HtmlNode> nodes , string expression )
        {
            return nodes.SelectMany( node => CssSelect( node , expression ) );
        }

        public static IEnumerable<HtmlNode> CssSelect( this HtmlNode node , string expression )
        {
            CssSelectorTokenizer tokenizer = new CssSelectorTokenizer();
            Token[] tokens = tokenizer.Tokenize( expression );
            CssSelectorExecutor<HtmlNode> executor = new CssSelectorExecutor<HtmlNode>( new List<HtmlNode> { node } , tokens.ToList() , new AgilityNavigationProvider() );

            return executor.GetElements();
        }

        public static IEnumerable<HtmlNode> CssSelect( this HtmlNode node , string[] expressions )
        {
            List<HtmlNode> elements = new List<HtmlNode>();
            foreach ( string expression in expressions )
            {
                List<HtmlNode> matchingElements = node.CssSelect( expression ).ToList();

                // Use a union to remove duplicates.
                elements = elements.Union( matchingElements ).ToList();
            }

            return elements.ToArray();
        }

        public static IEnumerable<HtmlNode> CssSelectAncestors( this IEnumerable<HtmlNode> nodes , string expression )
        {
            HtmlNode[] htmlNodes = nodes.SelectMany( node => CssSelectAncestors( node , expression ) ).ToArray();
            return htmlNodes.Distinct();
        }

        public static IEnumerable<HtmlNode> CssSelectAncestors( this HtmlNode node , string expression )
        {
            CssSelectorTokenizer tokenizer = new CssSelectorTokenizer();
            Token[] tokens = tokenizer.Tokenize( expression );
            CssSelectorExecutor<HtmlNode> executor = new CssSelectorExecutor<HtmlNode>( new List<HtmlNode> { node } , tokens.ToList() , new AgilityNavigationProvider() )
            {
                MatchAncestors = true
            };

            return executor.GetElements();
        }

    }
}