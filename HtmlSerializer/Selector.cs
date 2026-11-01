using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace HtmlSerializer
{
    public class Selector
    {
        public string? TagName { get; set; }
        public string? Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();

        public Selector? Parent { get; set; }
        public Selector? Child { get; set; }

        public static Selector Convert (string query)
        {
            var selectors = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            Selector root = new Selector();
            Selector currentSeletor = root;
            for(int i = 0; i < selectors.Length; i++)
            {
                string[] parts = Regex.Split(selectors[i], @"(?=[#.])");
                foreach (var part in parts)
                {
                    if (part.Length == 0) continue;
                    if (part.StartsWith('.'))
                        currentSeletor.Classes.Add(part.Substring(1));
                    else if(part.StartsWith('#'))
                        currentSeletor.Id = part.Substring(1);
                    else if(HtmlHelper.Instance.HtmlTags.Contains(part) || HtmlHelper.Instance.HtmlVoidTags.Contains(part))
                        currentSeletor.TagName = part;
                }
                if (i < selectors.Length - 1)
                {
                    var child = new Selector();
                    child.Parent = currentSeletor;
                    currentSeletor.Child = child;
                    currentSeletor = child;
                }
            }

            return root;
        }
    }
}
