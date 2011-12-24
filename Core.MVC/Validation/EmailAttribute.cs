using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Core.MVC.Validation
{
	public class EmailAttribute : ValidationAttribute, IClientValidatable
	{
		private const string EmailPattern = @"^[a-z0-9-_\.]{2,}@[a-z0-9-_\.]{2,}\.[a-z]{2,4}(\.[a-z]{2,4})?$";

		public string Localized
		{
			get { return ErrorMessage; }
			set { ErrorMessage = Localization.Extensions.Resource(new HttpContextWrapper(HttpContext.Current), value); }
		}

		public override bool IsValid(object value)
		{
			if (value == null)
				return true;

			var input = value.ToString();
			if (string.IsNullOrEmpty(input))
				return true;

			return Regex.IsMatch(input, EmailPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			yield return new ModelClientValidationRule {
				ErrorMessage = ErrorMessage,
				ValidationType = "match" + StringFunctions.RandomLetters(6),
				ValidationParameters = {
					{ "pattern", EmailPattern }
				}
			};
		}
	}
}