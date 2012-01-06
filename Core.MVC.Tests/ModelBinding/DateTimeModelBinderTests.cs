using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using Core.MVC.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.MVC.Tests.ModelBinding
{
	public class DateTimeModelBinderTests
	{
		[TestClass]
		public class BindModel
		{
			private readonly DateTimeModelBinder dateTimeModelBinder = new DateTimeModelBinder();

			[TestMethod]
			public void Default()
			{
				DateTime input = new DateTime(1964, 2, 12, 0, 0, 0, 0);

				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", input.ToString() }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(input, result.Value);
			}

			[TestMethod]
			public void DefaultDifferentModelName()
			{
				DateTime input = new DateTime(1964, 2, 12, 0, 0, 0, 0);

				var modelBindingContext = new ModelBindingContext {
					ModelName = "asdf",
					ValueProvider = new FormCollection {
						{ "Form.Date", input.ToString() }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
			}

			[TestMethod]
			public void DefaultEmptyFormCollection()
			{
				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection()
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
				AssertModelState(modelBindingContext, "form.Date", 1, 1);
			}

			[TestMethod]
			public void DefaultNullValue()
			{
				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", null }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
				AssertModelState(modelBindingContext, "form.Date", 1, 1);
			}

			[TestMethod]
			public void DefaultEmptyValue()
			{
				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", string.Empty }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
				AssertModelState(modelBindingContext, "form.Date", 1, 1);
			}

			[TestMethod]
			public void DefaultNotADate()
			{
				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", "asdf" }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
				AssertModelState(modelBindingContext, "form.Date", 1, 1);
			}

			[TestMethod]
			public void DefaultInvalidDate()
			{
				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", "2000/2/31" }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
				AssertModelState(modelBindingContext, "form.Date", 1, 1);
			}

			[TestMethod]
			public void DefaultLocalizationNLLeadingZeros()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");

				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", "05-07-2012" }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(new DateTime(2012, 7, 5), result.Value);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}

			[TestMethod]
			public void DefaultLocalizationNLWithoutLeadingZeros()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");

				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", "5-7-2012" }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(new DateTime(2012, 7, 5), result.Value);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}

			[TestMethod]
			public void DefaultLocalizationGBLeadingZeros()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", "05/07/2012" }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(new DateTime(2012, 7, 5), result.Value);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}

			[TestMethod]
			public void DefaultLocalizationGBWithoutLeadingZeros()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", "5/7/2012" }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(new DateTime(2012, 7, 5), result.Value);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}

			[TestMethod]
			public void DefaultLocalizationUSLeadingZeros()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", "07/05/2012" }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(new DateTime(2012, 7, 5), result.Value);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}

			[TestMethod]
			public void DefaultLocalizationUSWithoutLeadingZeros()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", "7/5/2012" }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(new DateTime(2012, 7, 5), result.Value);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}

			[TestMethod]
			public void DefaultLocalizationTRLeadingZeros()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");

				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", "05.07.2012" }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(new DateTime(2012, 7, 5), result.Value);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}

			[TestMethod]
			public void DefaultLocalizationTRWithoutLeadingZeros()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");

				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date", "5.7.2012" }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(new DateTime(2012, 7, 5), result.Value);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}


			[TestMethod]
			public void ThreeIntInputs()
			{
				DateTime input = new DateTime(1964, 2, 12, 0, 0, 0, 0);

				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", input.Year.ToString() },
						{ "Form.Date.Month", input.Month.ToString() },
						{ "Form.Date.Day", input.Day.ToString() }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(input, result.Value);
			}

			[TestMethod]
			public void ThreeIntInputsDifferentModelName()
			{
				DateTime input = new DateTime(1964, 2, 12, 0, 0, 0, 0);

				var modelBindingContext = new ModelBindingContext {
					ModelName = "asdf",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", input.Year.ToString() },
						{ "Form.Date.Month", input.Month.ToString() },
						{ "Form.Date.Day", input.Day.ToString() }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
			}

			[TestMethod]
			public void ThreeIntInputsEmptyFormColection()
			{
				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection()
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
				AssertModelState(modelBindingContext, "form.Date", 1, 1);
			}

			[TestMethod]
			public void ThreeIntInputsNullValues()
			{
				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", null },
						{ "Form.Date.Month", null },
						{ "Form.Date.Day", null }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
				AssertModelState(modelBindingContext, "form.Date", 1, 1);
			}

			[TestMethod]
			public void ThreeIntInputsEmptyValues()
			{
				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", string.Empty },
						{ "Form.Date.Month", string.Empty },
						{ "Form.Date.Day", string.Empty }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
				AssertModelState(modelBindingContext, "form.Date", 1, 1);
			}

			[TestMethod]
			public void ThreeIntInputsNotADate()
			{
				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", "a" },
						{ "Form.Date.Month", "b" },
						{ "Form.Date.Day", "c" }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
				AssertModelState(modelBindingContext, "form.Date", 1, 1);
			}

			[TestMethod]
			public void ThreeIntInputsInvalidDate()
			{
				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", "2000" },
						{ "Form.Date.Month", "2" },
						{ "Form.Date.Day", "31" }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
				AssertModelState(modelBindingContext, "form.Date", 1, 1);
			}

			[TestMethod]
			public void ThreeIntInputsMissingDay()
			{
				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", "2000" },
						{ "Form.Date.Month", "2" }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsFalse(result.HasValue);
				AssertModelState(modelBindingContext, "form.Date", 1, 1);
			}

			[TestMethod]
			public void ThreeIntInputsLocalizationNL()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");

				DateTime input = new DateTime(1964, 2, 12, 0, 0, 0, 0);

				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", input.Year.ToString() },
						{ "Form.Date.Month", input.Month.ToString() },
						{ "Form.Date.Day", input.Day.ToString() }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(input, result.Value);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}

			[TestMethod]
			public void ThreeIntInputsLocalizationGB()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

				DateTime input = new DateTime(1964, 2, 12, 0, 0, 0, 0);

				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", input.Year.ToString() },
						{ "Form.Date.Month", input.Month.ToString() },
						{ "Form.Date.Day", input.Day.ToString() }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(input, result.Value);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}

			[TestMethod]
			public void ThreeIntInputsLocalizationUS()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

				DateTime input = new DateTime(1964, 2, 12, 0, 0, 0, 0);

				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", input.Year.ToString() },
						{ "Form.Date.Month", input.Month.ToString() },
						{ "Form.Date.Day", input.Day.ToString() }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(input, result.Value);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}

			[TestMethod]
			public void ThreeIntInputsLocalizationTR()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");

				DateTime input = new DateTime(1964, 2, 12, 0, 0, 0, 0);

				var modelBindingContext = new ModelBindingContext {
					ModelName = "form.Date",
					ValueProvider = new FormCollection {
						{ "Form.Date.Year", input.Year.ToString() },
						{ "Form.Date.Month", input.Month.ToString() },
						{ "Form.Date.Day", input.Day.ToString() }
					}
				};

				DateTime? result = dateTimeModelBinder.BindModel(null, modelBindingContext) as DateTime?;

				Assert.IsTrue(result.HasValue);
				Assert.AreEqual(input, result.Value);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}

			private void AssertModelState(ModelBindingContext modelBindingContext, string key, int count, int errorCount)
			{
				Assert.AreEqual(count, modelBindingContext.ModelState.Count);
				Assert.IsTrue(modelBindingContext.ModelState.ContainsKey(key));

				ModelState modelState;
				Assert.IsTrue(modelBindingContext.ModelState.TryGetValue(key, out modelState));
				Assert.AreEqual(errorCount, modelState.Errors.Count);
			}
		}
	}
}