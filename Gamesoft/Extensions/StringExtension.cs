using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;

namespace Gamesoft.Extensions
{
    public static class StringExtension
    {
        public static string Titleize(this string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text).ToSentenceCase();
        }

        public static string ToSentenceCase(this string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }

        public static string DeleteHtml(this string text)
        {
            if (text == null || text.Length == 0)
            {
                return string.Empty;
            }
               
            return Regex.Replace(HttpUtility.HtmlDecode(text), "(<[^>]+>)", "");
        }
    }
}
