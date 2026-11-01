using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public static class HtmlElementExtensions
    {
        public static HashSet<HtmlElement> Query(this HtmlElement element, Selector selector)
        {
            var result = new HashSet<HtmlElement>();
            QueryRecursive(element, selector, result);
            return result;
        }
        private static void QueryRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> result)
        {
            var descendiants = element.Descendants().Skip(1).Where(d => QueryHelper(d, selector));
            if (selector.Child == null)
            {
                foreach (var d in descendiants)
                    result.Add(d);
            }
            else
            {
                foreach (var d in descendiants)
                    QueryRecursive(d, selector.Child, result);
            }
        }
        private static bool QueryHelper(HtmlElement element, Selector selector)
        {
            return (
                (selector.TagName == null || selector.TagName == element.Name)
              && (selector.Id == null || selector.Id == element.Id)
              && selector.Classes.All(c => element.Classes.Contains(c)));
        }
    }
}