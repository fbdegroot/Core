using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using Core.Extensions;

namespace Core.HTTP
{
	public static class Download
	{
		public static void File(string url, string path) {
			Directory.CreateDirectory(FileFunctions.GetDirectory(path));
			using (var client = new WebClient())
				client.DownloadFile(url, path);
		}

		public static Image Image(string url) {
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "GET";
			request.Timeout = 10000;

			using (var response = request.GetResponse()) {
				var stream = response.GetResponseStream();

				return Bitmap.FromStream(stream);
			}
		}

		public static XDocument Xml(string url) {
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "GET";
			request.Timeout = 10000;

			using (var response = request.GetResponse())
			using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
				return XDocument.Parse(reader.ReadToEnd());
		}

		private static Stream Stream(string url) {
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "GET";
			request.Timeout = 10000;

			using (var response = request.GetResponse())
				return response.GetResponseStream();
		}
	}
}
