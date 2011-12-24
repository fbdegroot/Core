using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Core.MVC.Validation
{
	public class ExactLengthAttribute : LengthAttribute
	{
		public ExactLengthAttribute(int size)
			: base(size, size)
		{
		}
	}

	public class LengthAttribute : ValidationAttribute, IClientValidatable
	{
		private readonly int min;
		private readonly int max;

		public string Localized
		{
			get { return ErrorMessage; }
			set { ErrorMessage = Localization.Extensions.Resource(new HttpContextWrapper(HttpContext.Current), value); }
		}

		public LengthAttribute(int max)
		{
			this.max = max;
		}

		public LengthAttribute(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		public override bool IsValid(object value)
		{
			if (value == null)
				return true;

			var input = value.ToString();
			if (min > 0 && input.Length < min)
				return false;

			if (input.Length > max)
				return false;

			return true;
		}

		public override string FormatErrorMessage(string name)
		{
			return min > 0
				? string.Format(ErrorMessage, min, max)
				: string.Format(ErrorMessage, max);
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			var rule = new ModelClientValidationRule {
				ErrorMessage = ErrorMessage,
				ValidationType = "length",
				ValidationParameters = {
					{ "max", max }
				}
			};

			if (min > 0)
				rule.ValidationParameters.Add("min", min);

			yield return rule;
		}
	}
}