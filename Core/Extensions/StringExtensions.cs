using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Extensions
{
	/// <summary>
	/// Supported Hash Algorithms
	/// </summary>
	public enum HashType
	{
		MD5,
		SHA1,
		SHA256,
		SHA384,
		SHA512
	}

	public enum PhoneNumberType
	{
		Invalid,
		Mobile,
		Netnumber3,
		Netnumber4,
		Commercial,
		CommercialLong
	}

	public static class StringExtensions
	{
		/* General regular expressions */
		static readonly Regex rgSafeUrl = new Regex(@"[^\w]+");
		static readonly Regex rgDoubleDash = new Regex(@"\-{2,}");
		static readonly Regex rgNonNumeric = new Regex(@"[^\d]+");
		static readonly Regex rgMobile = new Regex(@"^(06)(\d{3})(\d{3})(\d{2})");
		static readonly Regex rgNetnumber3 = new Regex(@"^(010|013|015|020|023|024|026|030|033|035|036|038|040|043|045|046|050|053|055|058|070|071|072|073|074|075|076|077|078|079|084|088)(\d{3})(\d{2})(\d{2})$");
		static readonly Regex rgNetnumber4 = new Regex(@"^(0111|0113|0114|0115|0117|0118|0161|0162|0164|0165|0166|0167|0168|0172|0174|0180|0181|0182|0183|0184|0186|0187|0222|0223|0224|0226|0227|0228|0229|0251|0252|0255|0294|0297|0299|0313|0314|0315|0316|0317|0318|0320|0321|0341|0342|0343|0344|0345|0346|0347|0348|0411|0412|0413|0416|0418|0475|0478|0481|0485|0486|0487|0488|0492|0493|0495|0497|0499|0511|0512|0513|0514|0515|0516|0517|0518|0519|0521|0522|0523|0524|0525|0527|0528|0529|0541|0543|0544|0545|0546|0547|0548|0561|0562|0566|0570|0571|0572|0573|0575|0577|0578|0591|0592|0593|0594|0595|0596|0597|0598|0599)(\d{3})(\d{3})$");
		static readonly Regex rgCommercial = new Regex(@"^(0\d0\d)(\d{2})(\d{2})$");
		static readonly Regex rgCommercialLong = new Regex(@"^(0\d0\d)(\d{3})(\d{4})$");
		static readonly Regex rgFullText = new Regex(@"[^a-z0-9\- ]");
		static readonly Regex rgEmail = new Regex(@"^[a-z0-9-_\.]{2,}@[a-z0-9-_\.]{2,}\.[a-z]{2,4}(\.[a-z]{2,4})?$");

		private static PhoneNumberType GetPhoneNumberType(string s) {
			if (rgMobile.IsMatch(s))
				return PhoneNumberType.Mobile;
			if (rgNetnumber3.IsMatch(s))
				return PhoneNumberType.Netnumber3;
			if (rgNetnumber4.IsMatch(s))
				return PhoneNumberType.Netnumber4;
			if (rgCommercial.IsMatch(s))
				return PhoneNumberType.Commercial;
			if (rgCommercialLong.IsMatch(s))
				return PhoneNumberType.CommercialLong;
			
			return PhoneNumberType.Invalid;
		}

		/// <summary>
		/// Formats a phonenumber by it's netnumber to a format like "(0123) 456 789". Only dutch phonenumbers are supported.
		/// </summary>
		public static string FormatPhoneNumber(this string s) {
			if (s == null) 
				return null;

			var input = rgNonNumeric.Replace(s, "");
			switch (GetPhoneNumberType(input)) {
				case PhoneNumberType.Mobile:
					return rgMobile.Replace(input, "($1) $2 $3 $4");
				case PhoneNumberType.Netnumber3:
					return rgNetnumber3.Replace(input, "($1) $2 $3 $4");
				case PhoneNumberType.Netnumber4:
					return rgNetnumber4.Replace(input, "($1) $2 $3");
				case PhoneNumberType.Commercial:
					return rgCommercial.Replace(input, "($1) $2 $3");
				case PhoneNumberType.CommercialLong:
					return rgCommercialLong.Replace(input, "($1) $2 $3");
				default:
					return s;
			}
		}

		/// <summary>
		/// Returns true if the current string is a valid phonenumber. Only dutch phonenumbers are supported.
		/// </summary>
		public static bool IsValidPhoneNumber(this string s) {
			if (string.IsNullOrWhiteSpace(s)) 
				return false;

			var input = rgNonNumeric.Replace(s, "");

			return GetPhoneNumberType(input) != PhoneNumberType.Invalid;
		}

		/// <summary>
		/// Returns true if the current string is a valid email address.
		/// </summary>
		public static bool IsValidEmail(this string s)
		{
			return !string.IsNullOrWhiteSpace(s) && rgEmail.IsMatch(s);
		}

		/// <summary>
		/// Returns true if the current string is null, empty or only contains spaces
		/// </summary>
		public static bool IsNull(this string s) {
			return s == null || s.Equals(string.Empty) || s.Replace(" ", "").Equals(string.Empty);
		}

		/// <summary>
		/// Injects the passed variables into the string, replacing all {N} tags
		/// </summary>
		/// <param name="variables">Variable to replace the {N} tag with</param>
		public static string Inject(this string s, params object[] variables) {
			return string.Format(s, variables);
		}

		/// <summary>
		/// Injects the variables in the dictionary into the string, replacing all {VARIABLE} tags
		/// </summary>
		public static string Inject(this string s, Dictionary<string, string> dictionary) {
			if (dictionary == null || dictionary.Count == 0)
				return s;

			foreach (var item in dictionary)
				s = s.Replace("{" + item.Key + "}", item.Value);

			return s;
		}

		/// <summary>
		/// Cuts the name at [length - 2] and adds " ...".
		/// </summary>
		/// <param name="length">The maximum length of the text</param>
		public static string ShortenName(this string s, int length) {
			if (s == null)
				return string.Empty;

			if (s.Length <= length)
				return s;

			return s.Substring(0, length - 2) + " ...";
		}

		/// <summary>
		/// Cuts the string at the last space between character 0 and [length - 2] and adds " ...".
		/// </summary>
		/// <param name="length">The maximum length of the text</param>
		public static string ShortenText(this string s, int length) {
			if (s == null)
				return string.Empty;

			if (s.Length <= length)
				return s;

			return s.Substring(0, s.Substring(0, length - 2).LastIndexOf(" ")) + " ...";
		}

		/// <summary>
		/// Properly capitalize the string, uppercasing the first letter.
		/// </summary>
		public static string Capitalize(this string s) {
			return s[0].ToString().ToUpperInvariant() + s.Substring(1).ToLowerInvariant();
		}

		/// <summary>
		/// Lowercasing and replaces all non-alphanumeric characters with a dash (removing the trailing and padding ones).
		/// </summary>
		public static string SafeURL(this string s) {
			if (string.IsNullOrWhiteSpace(s))
				return null;

			s = s.Trim();
			s = s.ToLower();
			s = s.Replace(" & ", " and ");
			s = StripAccents(s);
			s = rgSafeUrl.Replace(s, " ");
			s = s.Trim();
			s = s.Replace(" ", "-");
			s = rgDoubleDash.Replace(s, "-");
			return s;
		}

		/// <summary>
		/// Removes all non-alphanumeric characters
		/// </summary>
		public static string StripUnsafe(this string s) {
			return rgSafeUrl.Replace(s, "");
		}

		/// <summary>
		/// Removes the accents from all characters. For example: é is replaced with e.
		/// </summary>
		/// <returns></returns>
		public static string StripAccents(string input) {
			return Encoding.ASCII.GetString(Encoding.GetEncoding(1251).GetBytes(input));
		}

		/// <summary>
		/// Replaces all instances of "\r\n" with "&lt;br /&gt;"
		/// </summary>
		public static string ReplaceLineBreaks(this string s) {
			if (string.IsNullOrWhiteSpace(s))
				return s;

			return s.Replace("\r\n", "<br />")
					.Replace("\r", "<br />")
					.Replace("\n", "<br />");
		}

		/// <summary>
		/// Removes all instances of "\r\n"
		/// </summary>
		public static string StripLineBreaks(this string s) {
			return s.Replace("\r\n", " ");
		}

		/// <summary>
		/// Generates the MD5 hash based on the contents of the string
		/// </summary>
		// [Obsolete("Use Core.Hash(HashType) instead")]
		public static string MD5Hash(this string s) {
			var bytes = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(s));
			var builder = new StringBuilder();

			foreach (byte b in bytes)
				builder.Append(b.ToString("x2").ToLower());

			return builder.ToString();
		}

		/// <summary>
		/// Generates the hash based on the contents of the string and base64 encodes the result
		/// </summary>
		/// <param name="hashType">The hash algorithm to use, MD5 (24), SHA1 (28), SHA256 (44), SHA384 (64) or SHA512 (88)</param>
		public static string Hash(this string s, HashType hashType)
		{
			if (string.IsNullOrWhiteSpace(s))
				throw new ArgumentNullException();

			HashAlgorithm hashAlgorithm;
			switch (hashType) {
				case HashType.MD5:
					hashAlgorithm = new MD5CryptoServiceProvider();
					break;
				case HashType.SHA1:
					hashAlgorithm = new SHA1Managed();
					break;
				case HashType.SHA256:
					hashAlgorithm = new SHA256Managed();
					break;
				case HashType.SHA384:
					hashAlgorithm = new SHA384Managed();
					break;
				case HashType.SHA512:
					hashAlgorithm = new SHA512Managed();
					break;
				default:
					throw new Exception("Unknown Hash Algorithm");
			}

			using (hashAlgorithm) {
				var bytes = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(s));
				return Convert.ToBase64String(bytes);
			}
		}

		/// <summary>
		/// Extracts all unique words of 3 characters and longer and concatenates them with a space
		/// </summary>
		public static string ExtractKeywords(this string s) {
			var input = s;

			input = input.Trim().ToLowerInvariant();
			input = rgDoubleDash.Replace(input, "-");
			input = rgFullText.Replace(input, " ");

			var keywords = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
								.Where(k => k.Length >= 3)
								.Distinct()
								.OrderBy(k => k)
								.ToArray();

			return string.Join(" ", keywords);
		}

		/// <summary>
		/// Splits up the words and generates an AND full text query
		/// </summary>
		public static string GenerateFTQuery(this string s) {
			var words = s.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries)
						 .ToArray();

			return @"""" + string.Join(@"*"" AND """, words) + @"*""";
		}

		public static string Glue(this List<string> s, string seperator) {
			return string.Join(seperator, s.ToArray());
		}
	}
}