using System;

namespace Core.Extensions
{
	public static class DateTimeExtensions
	{
		public static string Descriptive(this DateTime dateTime, bool includingDayName = false) {
			var difference = DateTime.Now - dateTime;

			if (difference.TotalMinutes <= 10)
				return "zojuist";
			
			if (difference.TotalMinutes <= 60)
				return "afgelopen uur";
			
			if (difference.TotalMinutes <= 60 * 24)
				return Math.Floor(difference.TotalMinutes / 60d) + " uur geleden";
			
			if (difference.TotalMinutes <= 60 * 24 * 2)
				return "gisteren";
			
			if (difference.TotalMinutes <= 60 * 24 * 3) 
				return "eergisteren";

			if (!includingDayName)
				return dateTime.ToString(@"d MMMM yyyy o\m H:mm");

			return dateTime.ToString(@"dddd d MMMM yyyy o\m H:mm");
		}

		public static DateTime FirstSecond(this DateTime datetime) {
			return new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0);
		}

		public static DateTime LastSecond(this DateTime datetime) {
			return new DateTime(datetime.Year, datetime.Month, datetime.Day, 23, 59, 59);
		}

		public static DateTime StartOfWeek(this DateTime datetime) {
			var difference = datetime.DayOfWeek - DayOfWeek.Monday;
			if (difference < 0)
				difference += 7;

			return datetime.AddDays(-1 * difference).FirstSecond();
		}

		public static DateTime EndOfWeek(this DateTime datetime) {
			var difference = DayOfWeek.Sunday - datetime.DayOfWeek;
			if (difference < 0)
				difference += 7;

			return datetime.AddDays(difference).LastSecond();
		}
	}
}