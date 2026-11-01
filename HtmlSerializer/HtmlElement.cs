using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public class HtmlElement
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement? Parent { get; set; }
        public List<HtmlElement> Children { get; set; }
        public HtmlElement() {
            Attributes = new Dictionary<string, string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
            InnerHtml = string.Empty;
        }
        public HtmlElement(string name) : this() {
            Name = name;
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            HtmlElement current;
            while (queue.Count > 0)
            {
                current = queue.Dequeue();
                yield return current;
                foreach (HtmlElement child in current.Children)
                    queue.Enqueue(child);
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement? current = Parent;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public override string ToString()
        {
            return $"<{Name} id=\"{Id}\" class=\"{string.Join(" ", Classes)}\">";
        }
    }
}
