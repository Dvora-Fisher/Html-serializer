using HtmlSerializer;

HtmlSerialization serializer = new HtmlSerialization();
var html = await serializer.Load("https://hebrewbooks.org/beis");
var dom = serializer.Serialize(html);
var result = dom.Query(Selector.Convert("div p"));
result.ToList().ForEach(x => Console.WriteLine(x.ToString()));

Console.ReadLine();