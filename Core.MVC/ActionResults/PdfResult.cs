using System.Web.Mvc;

namespace Core.MVC.ActionResults
{
    public class PdfResult : ActionResult
    {
        private readonly byte[] bytes;

        public PdfResult(byte[] bytes) {
            this.bytes = bytes;
        }

        public override void ExecuteResult(ControllerContext context) {
            context.HttpContext.Response.ContentType = "application/pdf";
            context.HttpContext.Response.AddHeader("Content-Lenght", bytes.Length.ToString());
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.BinaryWrite(bytes);
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.End();
        }
    }
}