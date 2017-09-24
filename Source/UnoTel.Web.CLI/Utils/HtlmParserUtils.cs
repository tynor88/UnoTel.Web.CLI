using HtmlAgilityPack;
using System;
using System.Linq;

namespace UnoTel.Web.Cli.Utils
{
    internal static class HtlmParserUtils
    {
        internal static string GetSubscriptionNumberFromHtmlString(int subscriptionNumber, string html)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            return
                htmlDocument.DocumentNode
                .Descendants()
                .Where(x => x.InnerText == subscriptionNumber.ToString())
                .FirstOrDefault(x => x.FirstChild.Name == "a")
                .FirstChild
                .GetAttributeValue("href", string.Empty);
        }

        internal static string GetBalanceFromHtmlString(string html)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            return
                htmlDocument.DocumentNode
                .Descendants("div")
                .Where(d =>
                    d.Attributes.Contains("class") &&
                    d.Attributes["class"].Value.Contains("balanceNumber"))
                .Last()
                .InnerText
                .Replace("kr.", string.Empty)
                .Trim();
        }

        internal static bool SmsHasBeenSent(string html)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            bool messageWasSent =
                htmlDocument.DocumentNode
                .Descendants("span")
                .Where(d =>
                    d.Attributes.Contains("class") &&
                    d.Attributes["class"].Value.Contains("failureMessage"))
                .Single()
                .InnerText.Equals("Din websms er sendt", StringComparison.OrdinalIgnoreCase);

            return messageWasSent;
        }
    }
}
