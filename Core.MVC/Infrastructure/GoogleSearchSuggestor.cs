using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Core.MVC.Infrastructure
{
    public class GoogleSearchSuggestor
    {
        const string ApiUrl = "https://www.google.com/tbproxy/spell?lang=nl&hl=nl";

        public string GetSuggestion(string text) {
            var req = BuildXmlPost(BuildRequestXml(text));
            using (var response = req.GetResponse()) {
                var resultXml = GetXmlFromResponse(response);
                return CreateSuggestionText(text, resultXml);
            }
        }

        public IAsyncResult BeginGetSuggestion(string text, AsyncCallback cb) {
            var req = BuildXmlPost(BuildRequestXml(text));
            return req.BeginGetResponse(cb,
                new AsyncState { Text = text, WebRequest = req }
            );
        }

        public string EndGetSuggestion(IAsyncResult asyncResult) {
            var asyncState = (AsyncState)asyncResult.AsyncState;
            using (var response = asyncState.WebRequest.EndGetResponse(asyncResult)) {
                var resultXml = GetXmlFromResponse(response);
                return CreateSuggestionText(asyncState.Text, resultXml);
            }
        }

        private class AsyncState
        {
            public WebRequest WebRequest { get; set; }
            public string Text { get; set; }
        }

        private static string CreateSuggestionText(string text, XDocument spellingResult) {
            var originalText = text;
            var corrections = from c in spellingResult.Descendants("c")
                              where c.Attribute("s").Value == "1"
                              orderby int.Parse(c.Attribute("o").Value) descending
                              select new {
                                  Offset = int.Parse(c.Attribute("o").Value),
                                  OriginalLength = int.Parse(c.Attribute("l").Value),
                                  NewWords = c.Value.Split('\t')
                              };

            foreach (var correction in corrections) {
                text = text.Substring(0, correction.Offset)
                       + correction.NewWords.First()
                       + text.Substring(correction.Offset + correction.OriginalLength);
            }

            return text == originalText ? null : text;
        }

        private static XDocument GetXmlFromResponse(WebResponse response) {
            return XDocument.Load(XmlReader.Create(response.GetResponseStream()));
        }

        private static XDocument BuildRequestXml(string text) {
            return new XDocument(
                new XElement("spellrequest",
                    new XAttribute("textalreadyclipped", "0"),
                    new XAttribute("ignoreups", "1"),
                    new XAttribute("ignoredigits", "1"),
                    new XAttribute("ignoreallcaps", "0"),
                    new XElement("text", text)
                )
            );
        }

        private static WebRequest BuildXmlPost(XDocument content) {
            var bytes = Encoding.UTF8.GetBytes(content.ToString());

            WebRequest req = WebRequest.Create(ApiUrl);
            req.Method = "POST";

            // We have to call BeginGetRequestStream (not GetRequestStream) otherwise the
            // request will go into synchronous mode, where Begin/EndGetResponseStream will 
            // actually work synchronously. However, since there's no effective way to get
            // the request stream asynchronously (http://groups.google.com/group/microsoft.public.dotnet.framework/browse_thread/thread/5612cabc03b9b7b5),
            // we might as well block the thread until the request is at least opened.
            var dummyAsyncResult = req.BeginGetRequestStream(null, null);
            using (var reqStream = req.EndGetRequestStream(dummyAsyncResult)) {
                reqStream.Write(bytes, 0, bytes.Length);
            }
            return req;
        }
    }
}