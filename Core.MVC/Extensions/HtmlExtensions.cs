using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Core.MVC.Validation;

namespace Core.MVC.Extensions
{
	public static class HtmlExtensions
	{
		public static bool DevelopmentMode(this HtmlHelper helper) {
			return bool.Parse(ConfigurationManager.AppSettings["Development"]);
		}

		public static MvcHtmlString Style(this HtmlHelper helper, bool conditionA, string styleA, params string[] styles) {
			if (!conditionA && styles.Length == 0)
				return null;

			var styleList = new List<string>(styles);
			if (conditionA)
				styleList.Add(styleA);

			return MvcHtmlString.Create(@" style=""" + string.Join(";", styleList.ToArray()) + @"""");
		}

		public static MvcHtmlString Class(this HtmlHelper helper, bool conditionA, string classA, params string[] classes) {
			if (!conditionA && classes.Length == 0)
				return null;

			var classList = new List<string>(classes);
			if (conditionA)
				classList.Add(classA);

			return MvcHtmlString.Create(@" class=""" + string.Join(" ", classList.ToArray()) + @"""");
		}

		public static MvcHtmlString Class(this HtmlHelper helper, bool conditionA, string classA, bool conditionB, string classB, params string[] classes) {
			if (!conditionA && !conditionB && classes.Length == 0)
				return null;

			var classList = new List<string>(classes);
			if (conditionA)
				classList.Add(classA);
			if (conditionB)
				classList.Add(classB);

			return MvcHtmlString.Create(@" class=""" + string.Join(" ", classList.ToArray()) + @"""");
		}

		public static MvcHtmlString Class(this HtmlHelper helper, bool conditionA, string classA, bool conditionB, string classB, bool conditionC, string classC, params string[] classes) {
			if (!conditionA && !conditionB && classes.Length == 0)
				return null;

			var classList = new List<string>(classes);
			if (conditionA)
				classList.Add(classA);
			if (conditionB)
				classList.Add(classB);
			if (conditionC)
				classList.Add(classC);

			return MvcHtmlString.Create(@" class=""" + string.Join(" ", classList.ToArray()) + @"""");
		}

		public static MvcHtmlString Version(this HtmlHelper helper, Version version) {
			return MvcHtmlString.Create("v" + version.Major + "." + version.Minor + "." + version.Build);
		}

		public static string Config(this HtmlHelper helper, string key)
		{
			return ConfigurationManager.AppSettings[key];
		}

		public static MvcHtmlString Label<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
		{
			return Label(html, expression, null);
		}

		public static MvcHtmlString Label<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
		{
			return Label(html, expression, new RouteValueDictionary(htmlAttributes));
		}

		private static MvcHtmlString Label<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
			string innerHtml = metadata.DisplayName ?? metadata.PropertyName;

			if (innerHtml == null) {
				return MvcHtmlString.Empty;
			}

			if (IsRequired(expression, html)) {
				innerHtml += @" <span class=""mandatory"">*</span>";
			}

			string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

			TagBuilder tag = new TagBuilder("label");
			tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
			tag.InnerHtml = innerHtml;

			if (htmlAttributes != null) {
				tag.MergeAttributes(htmlAttributes);
			}

			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}

		private static bool IsRequired<T, V>(this Expression<Func<T, V>> expression, HtmlHelper<T> html)
		{
			var modelMetadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

			return ModelValidatorProviders.Providers
				.GetValidators(modelMetadata, html.ViewContext)
				.SelectMany(v => v.GetClientValidationRules())
				.Any(r => r.ValidationType == "required");
		}
	}
}