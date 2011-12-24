using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;

namespace Core.MVC.Localization
{
	public sealed class SqlResourceReader : IResourceReader
	{
		private IDictionary _resources;

		public SqlResourceReader(IDictionary resources)
		{
			_resources = resources;
		}

		IDictionaryEnumerator IResourceReader.GetEnumerator()
		{
			return _resources.GetEnumerator();
		}

		void IResourceReader.Close()
		{
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _resources.GetEnumerator();
		}

		void IDisposable.Dispose()
		{
			return;
		}
	}
}
