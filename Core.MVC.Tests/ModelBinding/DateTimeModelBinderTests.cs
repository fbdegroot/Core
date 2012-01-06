using System;
using System.Web.Mvc;
using Core.MVC.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.MVC.Tests.ModelBinding
{
	namespace DateTimeModelBinderTests
	{
		[TestClass]
		public class BindModel
		{
			private readonly DateTimeModelBinder dateTimeModelBinder = new DateTimeModelBinder();

			[TestMethod]
			public void Default()
			{
				DateTime input = new DateTime(1964, 2, 12, 0, 0, 0, 0);

				var bindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", input.ToString() }
					}
				};

				DateTime? result = (DateTime?)dateTimeModelBinder.BindModel(null, bindingContext);

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(input, result.Value);
			}

			[TestMethod]
			public void DefaultEmptyFormCollection()
			{
				var bindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection()
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, bindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
			}

			[TestMethod]
			public void DefaultNullValue()
			{
				var bindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", null }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, bindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
			}

			[TestMethod]
			public void DefaultEmptyValue()
			{
				var bindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", string.Empty }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, bindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
			}

			[TestMethod]
			public void DefaultNotADate()
			{
				var bindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", "asdf" }
					}
				};

				DateTime? result = (DateTime?)dateTimeModelBinder.BindModel(null, bindingContext);

				Assert.IsFalse(result.HasValue);
			}

			[TestMethod]
			public void DefaultInvalidDate()
			{
				var bindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", "2000/2/31" }
					}
				};

				DateTime? result = (DateTime?)dateTimeModelBinder.BindModel(null, bindingContext);

				Assert.IsFalse(result.HasValue);
			}



			[TestMethod]
			public void ThreeIntInputs()
			{
				DateTime input = new DateTime(1964, 2, 12, 0, 0, 0, 0);

				var bindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", input.Year.ToString() },
						{ "Form.Date.Month", input.Month.ToString() },
						{ "Form.Date.Day", input.Day.ToString() }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, bindingContext) as DateTime?;

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(input, result.Value);
			}

			[TestMethod]
			public void ThreeIntInputsEmptyFormColection()
			{
				var bindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection()
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, bindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
			}

			[TestMethod]
			public void ThreeIntInputsNullValues()
			{
				var bindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", null },
						{ "Form.Date.Month", null },
						{ "Form.Date.Day", null }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, bindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
			}

			[TestMethod]
			public void ThreeIntInputsEmptyValues()
			{
				var bindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", string.Empty },
						{ "Form.Date.Month", string.Empty },
						{ "Form.Date.Day", string.Empty }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, bindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
			}

			[TestMethod]
			public void ThreeIntInputsNotADate()
			{
				var bindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", "a" },
						{ "Form.Date.Month", "b" },
						{ "Form.Date.Day", "c" }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, bindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
			}

			[TestMethod]
			public void ThreeIntInputsInvalidDate()
			{
				var bindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", "2000" },
						{ "Form.Date.Month", "2" },
						{ "Form.Date.Day", "31" }
					}
				};

				DateTime? result = (DateTime?)dateTimeModelBinder.BindModel(null, bindingContext);

				Assert.IsFalse(result.HasValue);
			}

			[TestMethod]
			public void ThreeIntInputsMissingDay()
			{
				var bindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", "2000" },
						{ "Form.Date.Month", "2" }
					}
				};

				DateTime? result = (DateTime?)dateTimeModelBinder.BindModel(null, bindingContext);

				Assert.IsFalse(result.HasValue);
			}
		}
	}
}