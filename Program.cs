using System.Xml.Linq;

/// <summary>
/// Класс, представляющий событие в журнале
/// </summary>
public class Event
{
    public DateTime Date { get; set; }
    public string Result { get; set; }
    public string IpFrom { get; set; }
    public string Method { get; set; }
    public string UrlTo { get; set; }
    public int Response { get; set; }

    /// <summary>
    /// Конструктор класса Event
    /// </summary>
    public Event(DateTime date, string result, string ipFrom, string method, string urlTo, int response)
    {
        Date = date;
        Result = result;
        IpFrom = ipFrom;
        Method = method;
        UrlTo = urlTo;
        Response = response;
    }
}

/// <summary>
/// Класс, представляющий журнал событий
/// </summary>
public class Log
{
    public List<Event> Events { get; set; }

    /// <summary>
    /// Конструктор класса Log
    /// </summary>
    public Log()
    {
        Events = new List<Event>();
    }

    /// <summary>
    /// Метод для добавления события в журнал
    /// </summary>
    public void AddEvent(Event e)
    {
        Events.Add(e);
    }

    /// <summary>
    /// Метод для преобразования XML-документа в объекты Event и Log
    /// </summary>
    public static Log ParseXml(string xml)
    {
        Log log = new Log();
        XElement root = XElement.Parse(xml);
        foreach (XElement eventElement in root.Elements("event"))
        {
            DateTime date = DateTime.ParseExact(eventElement.Attribute("date").Value, "dd/MMM/yyyy:HH:mm:ss", null);
            string result = eventElement.Attribute("result").Value;
            string ipFrom = eventElement.Element("ip-from").Value;
            string method = eventElement.Element("method").Value;
            string urlTo = eventElement.Element("url-to").Value;
            int response = int.Parse(eventElement.Element("response").Value);

            Event e = new Event(date, result, ipFrom, method, urlTo, response);
            log.AddEvent(e);
        }
        return log;
    }
}

/// <summary>
/// Основной класс программы
/// </summary>
public class Program
{
    /// <summary>
    /// Главный метод программы
    /// </summary>
    public static void Main()
    {
        string xml = @"
<log>
    <event date=""27/May/1999:02:32:46"" result=""success"">
        <ip-from>195.151.62.18</ip-from>
        <method>GET</method>
        <url-to>/mise/</url-to>
        <response>200</response>
    </event>
    <event date=""27/May/1999:02:41:47"" result=""success"">
        <ip-from>195.209.248.12</ip-from>
        <method>GET</method>
        <url-to>soft.htm</url-to>
        <response>200</response>
    </event>
</log>";

        Log log = Log.ParseXml(xml);
        
        foreach (Event e in log.Events)
        {
            Console.WriteLine($"Date: {e.Date}, Result: {e.Result}, IP: {e.IpFrom}, Method: {e.Method}, URL: {e.UrlTo}, Response: {e.Response}");
        }
    }
}