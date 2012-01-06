using System;
using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Tests
{
	public class DateTimeFunctionsTests
	{
		[TestClass]
		public class TryCreate
		{
			[TestMethod]
			public void Default()
			{
				DateTime result;
				DateTimeFunctions.TryCreate("2000", "07", "05", out result);

				Assert.AreEqual(new DateTime(2000, 7, 5), result);
			}

			[TestMethod]
			public void OneEmptyString()
			{
				DateTime result;
				bool success = DateTimeFunctions.TryCreate("2000", "07", "", out result);

				Assert.IsFalse(success);
			}

			[TestMethod]
			public void AllEmptyStrings()
			{
				DateTime result;
				bool success = DateTimeFunctions.TryCreate("", "", "", out result);

				Assert.IsFalse(success);
			}

			[TestMethod]
			public void OneNull()
			{
				DateTime result;
				bool success = DateTimeFunctions.TryCreate("2000", "07", null, out result);

				Assert.IsFalse(success);
			}

			[TestMethod]
			public void AllNull()
			{
				DateTime result;
				bool success = DateTimeFunctions.TryCreate(null, null, null, out result);

				Assert.IsFalse(success);
			}

			[TestMethod]
			public void NotANumber()
			{
				DateTime result;
				bool success = DateTimeFunctions.TryCreate("a", "b", "c", out result);

				Assert.IsFalse(success);
			}

			[TestMethod]
			public void InvalidDate()
			{
				DateTime result;
				bool success = DateTimeFunctions.TryCreate("2000", "2", "31", out result);

				Assert.IsFalse(success);
			}


			// DateTimeFunctions.TryCreate is culture independant, but to be sure:

			[TestMethod]
			public void LocalizationNL()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");

				DateTime result;
				DateTimeFunctions.TryCreate("2000", "07", "05", out result);

				Assert.AreEqual(new DateTime(2000, 7, 5), result);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}

			[TestMethod]
			public void LocalizationGB()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

				DateTime result;
				DateTimeFunctions.TryCreate("2000", "07", "05", out result);

				Assert.AreEqual(new DateTime(2000, 7, 5), result);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}

			[TestMethod]
			public void LocalizationUS()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

				DateTime result;
				DateTimeFunctions.TryCreate("2000", "07", "05", out result);

				Assert.AreEqual(new DateTime(2000, 7, 5), result);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}

			[TestMethod]
			public void LocalizationTR()
			{
				CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");

				DateTime result;
				DateTimeFunctions.TryCreate("2000", "07", "05", out result);

				Assert.AreEqual(new DateTime(2000, 7, 5), result);

				Thread.CurrentThread.CurrentCulture = currentCultureInfo;
			}
		}
	}
}