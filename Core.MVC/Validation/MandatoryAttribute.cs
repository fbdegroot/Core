using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Core.MVC.Validation
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class MandatoryAttribute : ValidationAttribute, IClientValidatable
	{
		public string Localized
		{
			get { return ErrorMessage; }
			set { ErrorMessage = Localization.Extensions.Resource(new HttpContextWrapper(HttpContext.Current), value); }
		}

		public override bool IsValid(object value)
		{
			return value != null;
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			yield return new ModelClientValidationRule {
				ErrorMessage = ErrorMessage,
				ValidationType = "required"
			};
		}
	}
}