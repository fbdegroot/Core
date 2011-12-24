using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using Core.MVC.Extensions;

namespace Core.MVC.Controllers
{
    public class CaptchaController : Controller
    {
        readonly static Brush foreground = Brushes.Navy;
        readonly static Color background = Color.Silver;

        const int Width = 200, Height = 70;
        const int WarpFactor = 5;

        const string FontFamily = "Rockwell";

        const double AmpX = WarpFactor * Width / 100;
        const double AmpY = WarpFactor * Height / 85;
        const double FreqX = 2 * Math.PI / Width;
        const double FreqY = 2 * Math.PI / Height;

        public void Render(string challengeGuid) {
            var key = CaptchaExtensions.SessionKeyPrefix + challengeGuid;
            var solution = (string)HttpContext.Session[key];

        	if (solution == null) return;

        	using (var bmp = new Bitmap(Width, Height))
        	using (var g = Graphics.FromImage(bmp))
        	using (var font = new Font(FontFamily, 1f)) {
        		g.Clear(background);

        		SizeF finalSize;
        		SizeF testSize = g.MeasureString(solution, font);
        		float bestFontSize = Math.Min(Width / testSize.Width, Height / testSize.Height) * 0.95f;

        		using (var finalFont = new Font(FontFamily, bestFontSize))
        			finalSize = g.MeasureString(solution, finalFont);

        		g.PageUnit = GraphicsUnit.Point;
        		var textTopLeft = new PointF((Width - finalSize.Width) / 2,
        		                                (Height - finalSize.Height) / 2);

        		using (var path = new GraphicsPath()) {
        			path.AddString(solution, new FontFamily(FontFamily), 0, bestFontSize, textTopLeft, StringFormat.GenericDefault);

        			g.SmoothingMode = SmoothingMode.HighQuality;
        			g.FillPath(foreground, DeformPath(path));
        			g.Flush();

        			Response.ContentType = "image/png";
        			using (var memoryStream = new MemoryStream()) {
        				bmp.Save(memoryStream, ImageFormat.Png);
        				memoryStream.WriteTo(Response.OutputStream);
        			}
        		}
        	}
        }

    	static GraphicsPath DeformPath(GraphicsPath path) {
            var deformed = new PointF[path.PathPoints.Length];
            var rdm = new Random();
            var xSeed = rdm.NextDouble() * 2 * Math.PI;
            var ySeed = rdm.NextDouble() * 2 * Math.PI;

            for (var i = 0; i < path.PathPoints.Length; i++) {
                var original = path.PathPoints[i];
                var val = FreqX * original.X + FreqY * original.Y;
                var xOffset = (int)(AmpX * Math.Sin(val + xSeed));
                var yOffset = (int)(AmpY * Math.Sin(val + ySeed));
                deformed[i] = new PointF(original.X + xOffset, original.Y + yOffset);
            }

            return new GraphicsPath(deformed, path.PathTypes);
        }
    }
}