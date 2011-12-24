using System.Collections.Generic;

namespace Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<string, string> Add(this Dictionary<string, string> d, Dictionary<string, string> values)
        {
        	var dictionary = d != null
        	    ? new Dictionary<string, string>(d)
        	    : new Dictionary<string, string>();

            if (values != null)
                foreach (var value in values)
                    dictionary.Add(value.Key, value.Value);

            return dictionary;
        }
    }
}