namespace Core
{
	public static class DirectoryFunctions
	{
		/// <summary>
		/// Returns the last folder
		/// </summary>
		/// <param name="path">Path without filename and trailing slash</param>
		/// <returns></returns>
		public static string GetLastDirectory(string path)
		{
			var lastIndex = path.LastIndexOf(@"\");
			return lastIndex == 0 ? "" : path.Substring(lastIndex + 1);
		}
	}
}