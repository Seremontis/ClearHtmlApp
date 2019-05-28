using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ClearHtmlApp
{
    enum UserOption
    {
        Link,
        Images
    }

    class HtmlInfo
    {
        private string htmlContent;
        private string uri;
        UserOption userOption;
        string[] regexConditionalSearch = {
                @"\<a.{1,}href.{1,}",
                @"\<img.{1,}src.{1,}",
                };
        string[] regexConditionalClear;

        public HtmlInfo(int select,string page)
        {
            userOption = (UserOption)select;
            Regex regex = new Regex(@"^((http)|(https)).{1,}");
            if (regex.IsMatch(page))
            {
                uri = page;
            }
            else
            {
                uri = "http://"+page;
            }
            PrepareView();
        }

        private void PrepareView()
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Proxy = GlobalProxySelection.GetEmptyWebProxy();
                webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                try
                {
                    htmlContent = webClient.DownloadString(uri);
                }
                catch (Exception)
                {

                    throw;
                }              
            }
        }

        public List<string> ResultOfPage()
        {
            Console.WriteLine("Proszę czekać...");
            Console.WriteLine(".......");
            var resultcheck = RegexCheck(htmlContent);
            var listToReturn = UrlFilter(resultcheck);
            return listToReturn;
        }

        private MatchCollection RegexCheck(string html)
        {
            Regex regex = new Regex(regexConditionalSearch[(int)userOption]);
            MatchCollection matching = regex.Matches(html);
            return matching;
        }

        private List<string> UrlFilter(MatchCollection matches)
        {
            List<string> textoutput = new List<string>();
            if (matches.Count > 0)
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    textoutput.Add(ClearString(matches[i].ToString()));
                }
            }            
            else
            {
                textoutput.Add("Brak wyników");
            }
            return textoutput;
        }

        private string ClearString(string line)
        {
            string[] RegexToClearString = {
                @"([\<]|[\<][\/])[A-Za-z]{1,}[\>]",
                @"[\<](img).(src)[\=].",
                @".{2}(alt).{1,}[\/][\>]" };
            foreach (var item in RegexToClearString)
            {
                line = Regex.Replace(line, item, "");
            }
            return line;
        }

    }
}
