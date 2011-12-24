namespace Core
{
	public static class FileFunctions
	{
		public static string GetExtension(string path)
		{
			var lastIndex = path.LastIndexOf(".");
			return lastIndex == -1 ? "" : path.Substring(lastIndex + 1);
		}

		public static string GetDirectory(string path)
		{
			var lastIndex = path.LastIndexOf(@"\");
			return lastIndex == -1 ? "" : path.Substring(0, lastIndex);
		}

		public static string GetFile(string path)
		{
			var lastIndex = path.LastIndexOf(@"\");
			return lastIndex == -1 ? path : path.Substring(lastIndex + 1);
		}
	}
}