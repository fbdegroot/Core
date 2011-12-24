using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Core.MVC.Validation
{
	public class MatchAttribute : RegexAttribute
	{
		public MatchAttribute(string pattern)
			: base(pattern, true)
		{
		}
	}

	public class NotMatchAttribute : RegexAttribute
	{
		public NotMatchAttribute(string pattern)
			: base(pattern, false)
		{
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
	public class RegexAttribute : ValidationAttribute, IClientValidatable
	{
		private readonly string pattern;
		private readonly bool mustMatch;
		private readonly object typeId = new object();

		public string Localized
		{
			get { return ErrorMessage; }
			set { ErrorMessage = Localization.Extensions.Resource(new HttpContextWrapper(HttpContext.Current), value); }
		}

		public RegexAttribute(string pattern, bool mustMatch)
		{
			this.pattern = pattern;
			this.mustMatch = mustMatch;
		}

		public override bool IsValid(object value)
		{
			if (value == null)
				return true;

			var input = value.ToString();
			if (string.IsNullOrEmpty(input))
				return true;

			return Regex.IsMatch(input, pattern) == mustMatch;
		}

		public override object TypeId
		{
			get { return typeId; }
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			yield return new ModelClientValidationRule {
				ErrorMessage = ErrorMessage,
				ValidationType = (mustMatch ? "match" : "notmatch") + StringFunctions.RandomLetters(6),
				ValidationParameters = {
					{ "pattern", pattern }
				}
			};
		}
	}
}