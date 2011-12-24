using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace Core.MVC.ActionResults
{
    public class XmlResult : ActionResult
    {
        private readonly XDocument document;

        public XmlResult(XDocument document) {
            this.document = document;
        }

        public override void ExecuteResult(ControllerContext context) {
            context.HttpContext.Response.HeaderEncoding = Encoding.GetEncoding(document.Declaration.Encoding);
            context.HttpContext.Response.ContentType = "text/xml";

            using (var writer = new XmlTextWriter(new StreamWriter(context.HttpContext.Response.OutputStream))) {
                writer.Formatting = Formatting.Indented;
                document.Save(writer);
            }
        }
    }
}