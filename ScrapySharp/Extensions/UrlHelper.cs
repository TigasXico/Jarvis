using System;
using System.Text.RegularExpressions;

namespace ScrapySharp.Extensions
{
    public static class UrlHelper
    {
        private static readonly Regex basePathRegex = new Regex("(?<scheme>(http[s]?[:]//)?)(?<site>[^/]+).*", RegexOptions.Compiled);

        public static Uri Combine(this Uri uri, string path)
        {
            string url = uri.ToString();
            return CombineUrl(url, path);
        }

        public static Uri CombineUrl(this string url, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return new Uri(url);
            }

            if (path.StartsWith("/"))
            {
                Match match = basePathRegex.Match(url);
                if (match.Success)
                {
                    string scheme = match.Groups["scheme"].Value;
                    string site = match.Groups["site"].Value;

                    return new Uri(scheme + site + path);
                }
            }

            if (!url.EndsWith("/"))
            {
                url += '/';
            }

            string combined;
            if (url.EndsWith("/") && path.StartsWith("/"))
            {
                combined = url + path.Substring(1);
            }
            else
            {
                combined = url + path;
            }

            return new Uri(combined);
        }
    }
}