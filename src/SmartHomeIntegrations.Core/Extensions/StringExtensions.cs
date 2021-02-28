using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SmartHomeIntegrations.Core.Extensions
{
    public static class StringExtensions
    {
        public static int GetProductivityScore(this string html)
        {
            var score = Regex.Split(html.Substring(html.IndexOf("efficiency_percent")), @"\D+")
                .FirstOrDefault(s => string.IsNullOrWhiteSpace(s) == false);
            return Convert.ToInt32(score);
        }
    }
}
