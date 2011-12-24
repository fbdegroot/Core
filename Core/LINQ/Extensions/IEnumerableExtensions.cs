using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects.DataClasses;
using System.Diagnostics.Contracts;

namespace Core.LINQ.Extensions
{
	public static class IEnumerableExtensions
	{
		public static EntityCollection<T> ToEntityCollection<T>(this IEnumerable<T> source) where T : class, IEntityWithRelationships
		{
			Contract.Requires(source != null);

			var collection = new EntityCollection<T>();
			foreach (var item in source)
				collection.Add(item);

			return collection;
		}
	}
}