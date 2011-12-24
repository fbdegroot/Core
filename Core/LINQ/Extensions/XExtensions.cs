using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Core.Extensions;

namespace Core.LINQ.Extensions
{
	public static class XExtensions
	{
		static readonly Regex rgValidateTime = new Regex(@"^[0-2]\d:[0-5]\d$");

		[ThreadStatic]
		static CultureInfo cultureInfo = CultureInfo.CurrentCulture;
		public static CultureInfo CultureInfo
		{
			get
			{
				return cultureInfo;
			}
			set
			{
				cultureInfo = value;
			}
		}

		public static T ReadAttribute<T>(this XElement e, string name)
		{
			return Read<T>(e.Attribute(name));
		}
		public static T ReadElement<T>(this XElement e, string name)
		{
			return Read<T>(e.Element(name));
		}

		private static T Read<T>(object e)
		{
			var type = typeof(T);
			string value = string.Empty;

			if (e is XAttribute)
				value = (e as XAttribute).Value;
			else if (e is XElement)
				value = (e as XElement).Value;

			if (e == null || string.IsNullOrWhiteSpace(value))
				return default(T);

			if (type == typeof(int))
				return (T)ReadInt32(value);
			if (type == typeof(int?))
				return (T)ReadInt32(value, true);
			if (type == typeof(decimal))
				return (T)ReadDecimal(value);
			if (type == typeof(decimal?))
				return (T)ReadDecimal(value, true);
			if (type == typeof(DateTime))
				return (T)ReadDateTime(value);
			if (type == typeof(DateTime?))
				return (T)ReadDateTime(value, true);
			if (type == typeof(string))
				return (T)ReadString(value);
			
			return default(T);
		}

		private static object ReadInt32(string s, bool nullable = false)
		{
			int result;
			if (int.TryParse(s, NumberStyles.Any, cultureInfo, out result))
				return result;
			
			if (nullable == false) 
				return default(int); 
			
			return null;
		}
		private static object ReadDecimal(string s, bool nullable = false)
		{
			decimal result;
			if (decimal.TryParse(s, NumberStyles.Any, cultureInfo, out result))
				return result;
			
			if (nullable == false)
				return default(decimal); 
			
			return null;
		}
		private static object ReadDateTime(string s, bool nullable = false)
		{
			var content = s;
			if (rgValidateTime.IsMatch(s))
				content = "2000-01-01 {0}:{1}:00".Inject(content.Substring(0, 2), content.Substring(3, 2));

			DateTime result;
			if (DateTime.TryParse(content, cultureInfo, DateTimeStyles.None, out result))
				return result;
			
			if (nullable == false) 
				return default(DateTime); 
			
			return null;
		}
		private static object ReadString(this string s)
		{
			if (string.IsNullOrEmpty(s))
				return null;
			
			return s.ToString(cultureInfo);
		}
	}
}