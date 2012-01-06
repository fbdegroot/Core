using System.Web.Mvc;

namespace Core.MVC.ModelBinding
{
	public static class ModelBindingContextExtensions
	{
		public static bool TryGetValue<T>(this ModelBindingContext modelBindingContext, out T result)
		{
			return TryGetValue(modelBindingContext, null, out result);
		}

		public static bool TryGetValue<T>(this ModelBindingContext modelBindingContext, string suffix, out T result)
		{
			try {
				ValueProviderResult valueProviderResult = modelBindingContext.ValueProvider.GetValue(modelBindingContext.ModelName + suffix);
				if (valueProviderResult == null) {
					result = default(T);
					return false;
				}

				result = (T)valueProviderResult.ConvertTo(typeof(T));
				return true;
			}
			catch {
				result = default(T);
				return false;
			}
		}
	}
}
