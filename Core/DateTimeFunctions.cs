using System;

namespace Core
{
	public static class DateTimeFunctions
	{
		public static bool TryCreate(string year, string month, string day, out DateTime result)
		{
			int y, m, d;

			if (int.TryParse(year, out y) && int.TryParse(month, out m) && int.TryParse(day, out d)) {
				try {
					result = new DateTime(y, m, d);
					return true;
				}
				catch {
				}
			}

			result = new DateTime();
			return false;
		}
	}
}