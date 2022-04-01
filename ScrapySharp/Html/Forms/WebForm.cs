using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HtmlAgilityPack;

using ScrapySharp.Html.Dom;
using ScrapySharp.Network;

namespace ScrapySharp.Html.Forms
{
    public class WebForm
    {
        private HttpVerb method;
        private string action;

        public WebForm()
        {
            FormFields = new List<FormField>();
            method = HttpVerb.Post;
            action = string.Empty;
        }

        public WebForm(HtmlNode html)
        {
            AgilityNodeParser nodeParser = new AgilityNodeParser(html);
            Initialize(nodeParser);
        }

        public WebForm(HElement html)
        {
            HElementNodeParser nodeParser = new HElementNodeParser(html);
            Initialize(nodeParser);
        }

        private void Initialize<T>(IHtmlNodeParser<T> nodeParser)
        {
            FormFields = ParseFormFields(nodeParser);
            ParseAction(nodeParser);
            ParseMethod(nodeParser);
        }

        private void ParseMethod<T>(IHtmlNodeParser<T> nodeParser)
        {
            string value = nodeParser.GetAttributeValue("method");

            if (!string.IsNullOrEmpty(value) && value.Equals("get"))
            {
                method = HttpVerb.Get;
            }
            else
            {
                method = HttpVerb.Post;
            }
        }

        private void ParseAction<T>(IHtmlNodeParser<T> nodeParser)
        {
            action = nodeParser.GetAttributeValue("action");
        }

        internal static List<FormField> ParseFormFields<T>(IHtmlNodeParser<T> node)
        {
            IEnumerable<FormField> inputs = from input in node.CssSelect("input")
                             let value = input.GetAttributeValue("value")
                             let type = input.GetAttributeValue("type")
                         where type != "checkbox" && type != "radio"
                         select new FormField
                         {
                             Name = input.GetAttributeValue("name"),
                             Value = string.IsNullOrEmpty(value) ? input.InnerText : value
                         };

            IEnumerable<FormField> checkboxes = from input in node.CssSelect("input[type=checkbox]")
                         let value = input.GetAttributeValue("value")
                             where input.Attributes.AllKeys.Contains("checked")
                         select new FormField
                         {
                             Name = input.GetAttributeValue("name"),
                             Value = string.IsNullOrEmpty(value) ? input.InnerText : value
                         };

            IEnumerable<FormField> radios = from input in node.CssSelect("input[type=radio]")
                         let value = input.GetAttributeValue("value")
                             where input.Attributes.AllKeys.Contains("checked")
                         select new FormField
                         {
                             Name = input.GetAttributeValue("name"),
                             Value = string.IsNullOrEmpty(value) ? input.InnerText : value
                         };

            IEnumerable<FormField> selects = from @select in node.CssSelect("select")
                          let name = @select.GetAttributeValue("name")
                          let option =
                              @select.CssSelect("option").FirstOrDefault(o => o.Attributes["selected"] != null) ??
                              @select.CssSelect("option").FirstOrDefault()
                          let value = option.GetAttributeValue("value")
                          select new FormField
                          {
                              Name = name,
                              Value = string.IsNullOrEmpty(value) ? option.InnerText : value
                          };

            return inputs.Concat(selects).Concat(checkboxes).Concat(radios).ToList();
        }

        public List<FormField> FormFields { get; set; }

        public string SerializeFormFields()
        {
            StringBuilder builder = new StringBuilder();
            FormField[] fields = FormFields.ToArray();

            for (int i = 0; i < fields.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(fields[i].Name))
                {
                    continue;
                }

                if (i > 0)
                {
                    builder.Append('&');
                }

                builder.AppendFormat("{0}={1}", Uri.EscapeDataString(fields[i].Name), Uri.EscapeDataString(fields[i].Value));
            }

            return builder.ToString();
        }

        public string this[string key]
        {
            get
            {
                FormField field = FormFields.FirstOrDefault(f => f.Name == key);
                return field?.Value;
            }
            set
            {
                FormField field = FormFields.FirstOrDefault(f => f.Name == key);
                if (field != null)
                {
                    FormFields.Remove(field);
                }

                FormFields.Add(new FormField { Name = key, Value = value });
            }
        }

        public void Submit(ScrapingBrowser browser, Uri url, HttpVerb verb)
        {
            browser.NavigateTo(url, verb, SerializeFormFields());
        }

        public void Submit(ScrapingBrowser browser, Uri url)
        {
            browser.NavigateTo(url, method, SerializeFormFields());
        }

        public HttpVerb Method
        {
            get => method;
            set => method = value;
        }

        public string Action
        {
            get => action;
            set => action = value;
        }
    }
}