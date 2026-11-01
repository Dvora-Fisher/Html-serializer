using HtmlSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public class HtmlSerialization
    {
        static HttpClient client = new HttpClient();
        public async Task<string> Load(string url)
        {
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }

        public HtmlElement Serialize(string html)
        {
            var clearHtml = Regex.Replace(html, @"\s+", " ");

            var htmlTags = Regex.Split(clearHtml, @"(<.*?>)")
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s));

            HtmlElement root = new HtmlElement();
            HtmlElement currentTag = root;

            foreach (var tag in htmlTags)
            {
                if (!tag.StartsWith('<'))
                {
                    currentTag.InnerHtml += currentTag.InnerHtml.Length > 0 ? " " + tag : tag;
                    continue;
                }
                string tagContent = tag.Substring(1, tag.Length - 2).Trim();

                bool isSelfClosing = tagContent.EndsWith('/');
                string cleanTag = isSelfClosing ? tagContent.TrimEnd('/').Trim() : tagContent;

                int spaceIndex = cleanTag.IndexOf(' ');
                string firstWord = spaceIndex == -1 ? cleanTag : cleanTag.Substring(0, spaceIndex);
                firstWord = firstWord.ToLower();
                bool isVoidTag = isSelfClosing || HtmlHelper.Instance.HtmlVoidTags.Contains(firstWord);

                //closing tag
                if (firstWord.StartsWith('/'))
                {
                    if (firstWord == "/html")
                        break;
                    if (currentTag.Parent != null)
                        currentTag = currentTag.Parent;
                }

                else if (HtmlHelper.Instance.HtmlTags.Contains(firstWord) || isVoidTag)
                {
                    var thisTag = new HtmlElement(firstWord);
                    currentTag.Children.Add(thisTag);
                    thisTag.Parent = currentTag;

                    string rest = spaceIndex == -1 ? "" : cleanTag.Substring(spaceIndex + 1);

                    var attributes = new Regex("([\\w-]+)\\s*=\\s*\"(.*?)\"").Matches(rest);
                    foreach (Match attr in attributes)
                    {
                        string name = attr.Groups[1].Value;
                        string value = attr.Groups[2].Value;
                        if (name == "class")
                            thisTag.Classes = value.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                        else if (name == "id")
                            thisTag.Id = value;
                        else
                            thisTag.Attributes[name] = value;
                    }
                    if (!isVoidTag)
                    {
                        currentTag = thisTag;
                    }
                }

            }
            return root;
        }
    }
}
