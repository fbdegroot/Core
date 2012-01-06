using System;
using System.Web.Mvc;

namespace Core.MVC.ModelBinding
{
	public class DateTimeModelBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			if (bindingContext == null) {
				throw new ArgumentNullException("bindingContext");
			}

			DateTime result;

			// datetime split over 3 int inputs
			if (bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName + ".Year") &&
				bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName + ".Month") &&
				bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName + ".Day")) {

				string year, month, day;

				if (bindingContext.TryGetValue(".Year", out year) &&
					bindingContext.TryGetValue(".Month", out month) &&
					bindingContext.TryGetValue(".Day", out day)) {

					string date = string.Format("{0}/{1}/{2}", year, month, day);

					if (DateTime.TryParse(date, out result)) {
						return result;
					}

					bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format(@"""{0}"" is not a valid date", date));
					return null;
				}
			}

			// normal datetime
			if (bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName)) {
				string date;

				if (bindingContext.TryGetValue(out date)) {
					if (DateTime.TryParse(date, out result)) {
						return result;
					}

					bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format(@"""{0}"" is not a valid date", date));
					return null;
				}
			}

			bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Unable to bind the DateTime");
			return null;
		}
	}
}