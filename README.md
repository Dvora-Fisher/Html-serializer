# Html Serializer

כלי ב-C# (.NET 8) שקורא דף אינטרנט, ממיר את ה-HTML שלו לעץ של אובייקטים, ומאפשר לחפש בעץ אלמנטים לפי CSS Selector.
זו התשתית שאפשר לבנות עליה Crawler.

## דוגמת שימוש

```csharp
var serializer = new HtmlSerialization();
var html = await serializer.Load("https://example.com");
var dom = serializer.Serialize(html);

var result = dom.Query(Selector.Convert("div p.class-name"));
foreach (var element in result)
    Console.WriteLine(element);
```

## סלקטורים נתמכים

| סלקטור | משמעות |
|---|---|
| `div` | לפי שם תגית |
| `#main` | לפי id |
| `.box` | לפי class |
| `div#main.box` | כמה תנאים על אותו אלמנט |
| `div .box p` | צאצאים בכל עומק (רווח = "בתוך") |

## מבנה הפרויקט

| קובץ | תפקיד |
|---|---|
| `HtmlSerialization.cs` | טעינת הדף ובניית העץ |
| `HtmlElement.cs` | אלמנט בעץ, כולל `Descendants` ו-`Ancestors` |
| `Selector.cs` | סלקטור ופירוק מחרוזת שאילתה לשרשרת סלקטורים |
| `HtmlElementExtensions.cs` | פונקציית ההרחבה `Query` |
| `HtmlHelper.cs` | רשימות תגיות ה-HTML, נטענות מקבצי JSON |
| `seed/` | קבצי ה-JSON של התגיות |

## החלטות עיצוב

- **בניית העץ בלולאה ולא ברקורסיה:** HTML גדול עלול לגרום ל-StackOverflow.
- **`Descendants` עם `Queue` ו-`yield return`:** מעבר על העץ לרוחב (BFS), בלי רקורסיה, עם החזרה עצלה של האיברים.
- **`HtmlHelper` כ-Singleton:** קבצי ה-JSON נטענים פעם אחת בלבד.
- **`Query` כפונקציית הרחבה:** לוגיקת החיפוש מופרדת ממחלקת הנתונים.
- **`HashSet` לתוצאות:** מונע כפילויות כשאלמנט מתאים לסלקטור דרך כמה אבות.
- **`HttpClient` סטטי:** מופע אחד לכל התוכנית, כדי לא למצות חיבורים.

## הרצה

פותחים את `HtmlSerializer.sln` ב-Visual Studio 2022 ומריצים (Ctrl+F5).
