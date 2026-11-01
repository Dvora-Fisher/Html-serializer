using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public class HtmlHelper
    {
        private static readonly HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }
        private HtmlHelper() {
            string htmlTagsJson = File.ReadAllText("seed/HtmlTags.json");
            string htmlVoidTagsJson = File.ReadAllText("seed/HtmlVoidTags.json");
            HtmlTags = JsonSerializer.Deserialize<string[]>(htmlTagsJson) ?? Array.Empty<string>();
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(htmlVoidTagsJson) ?? Array.Empty<string>();
        }
    }
}