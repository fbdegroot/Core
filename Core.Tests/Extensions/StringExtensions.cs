using Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Tests.Extensions
{
	[TestClass]
	public class StringExtensions
	{
		[TestMethod]
		public void FormatPhoneNumber()
		{
			// geldige telefoonnummers
			Assert.AreEqual("(06) 123 456 78", "0612345678".FormatPhoneNumber());
			Assert.AreEqual("(010) 123 45 67", "0101234567".FormatPhoneNumber());
			Assert.AreEqual("(0182) 123 456", "0182123456".FormatPhoneNumber());
			Assert.AreEqual("(0800) 12 34", "08001234".FormatPhoneNumber());
			Assert.AreEqual("(0900) 12 34", "09001234".FormatPhoneNumber());
			Assert.AreEqual("(0909) 123 4567", "09091234567".FormatPhoneNumber());
			Assert.AreEqual("(010) 452 16 56", "010 4521656".FormatPhoneNumber());
			Assert.AreEqual("(010) 452 16 56", "010 45 21 656".FormatPhoneNumber());
			Assert.AreEqual("(010) 452 16 56", "010 452 1656".FormatPhoneNumber());
			Assert.AreEqual("(010) 452 16 56", "010 4521 656".FormatPhoneNumber());
			Assert.AreEqual("(010) 452 16 56", "(010)4521 656".FormatPhoneNumber());
			Assert.AreEqual("(010) 452 16 56", "(010) 452 16 56".FormatPhoneNumber());
			Assert.AreEqual("(010) 452 16 56", "(010) - 452 - 16 56".FormatPhoneNumber());

			// ongeldige telefoonnummers
			Assert.AreEqual("0909123456789", "0909123456789".FormatPhoneNumber());
			Assert.AreEqual("geen telefoonnummer", "geen telefoonnummer".FormatPhoneNumber());
			Assert.AreEqual("0000000000", "0000000000".FormatPhoneNumber());
			Assert.AreEqual("", "".FormatPhoneNumber());
			Assert.AreEqual(null, ((string)null).FormatPhoneNumber());
		}
	}
}