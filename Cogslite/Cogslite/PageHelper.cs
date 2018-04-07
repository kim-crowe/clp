using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cogslite
{
    public static class PageHelper
    {
        public static string WhenContains(this IDictionary<string, object> dictionary, string key, string positiveResult, string negativeResult = "")
        {
            if (dictionary.ContainsKey(key) && dictionary[key] != null)
                return positiveResult;
            return negativeResult;
        }

        public static IHtmlContent AntiForgeryTokenOnly(this IHtmlHelper helper)
        {
            var stringWriter = new System.IO.StringWriter();
            helper.AntiForgeryToken().WriteTo(stringWriter, HtmlEncoder.Default);
            var antiForgeryInputTag = stringWriter.ToString();
            var tokenValue = XElement.Parse(antiForgeryInputTag).Attribute("value").Value;
            return new HtmlString(tokenValue);
        }
    }
}
