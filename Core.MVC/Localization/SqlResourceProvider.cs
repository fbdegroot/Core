using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Web.Compilation;
using System.Web.Configuration;

namespace Core.MVC.Localization
{
	public class SqlResourceProvider : IResourceProvider
	{
		private bool loaded;
		private readonly string projectName;
		private readonly string namespaceName;
		private readonly string className;
		private readonly Dictionary<string, string> dictionary = new Dictionary<string, string>();
		private readonly SqlResourceProviderSection config;

		public SqlResourceProvider(string path)
		{
			var parts = path.Split('/');
			projectName = parts[0];
			namespaceName = parts[1];
			className = parts[2];

			config = WebConfigurationManager.GetWebApplicationSection("sqlResourceProvider") as SqlResourceProviderSection;
		}

		private void LoadResourceCache()
		{
			lock (dictionary) {
				if (loaded)
					return;

				// load data from db
				using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[config.ConnectionStringName].ConnectionString))
				using (var command = new SqlCommand()) {
					connection.Open();

					command.Connection = connection;
					command.CommandText = string.Format(@"select [{0}], [{1}], [{2}]
														  from [{3}].[{4}]
														  where [{5}] = '{6}' and [{7}] = '{8}' and [{9}] = '{10}'", 
						config.LanguageColumn, 
						config.KeyColumn, 
						config.ValueColumn,
						config.TableNamespace,
						config.Table, 
						config.ProjectColumn, 
						projectName,
						config.NamespaceColumn, 
						namespaceName,
						config.ClassColumn,
						className);

					using (var reader = command.ExecuteReader()) {
						while (reader.Read()) {
							dictionary.Add(string.Format("{0}.{1}", reader.GetString(1), reader.GetString(0)), reader.GetString(2));
						}
					}
				}

				loaded = true;
			}
		}

		object IResourceProvider.GetObject(string resourceKey, CultureInfo culture)
		{
			if (loaded == false)
				LoadResourceCache();

			string cultureShort = culture.Name.Substring(0, 2);
			string key = string.Format("{0}.{1}", resourceKey, cultureShort);
			if (dictionary.ContainsKey(key) == false)
				throw new KeyNotFoundException(string.Format(@"Key ""{0}/{1}/{2}.{3}.{4}"" was not found in the dictionary", projectName, namespaceName, className, resourceKey, cultureShort));

			return dictionary[key];
		}

		IResourceReader IResourceProvider.ResourceReader
		{
			get
			{
				string cultureName = null;
				CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
				if (!String.Equals(currentUICulture.Name, CultureInfo.InstalledUICulture.Name)) {
					cultureName = currentUICulture.Name;
				}

				return new SqlResourceReader(dictionary);
			}
		}

		public static SqlResourceIdentity GetClassKey(string expression)
		{
			if (expression == null)
				throw new ArgumentNullException("expression");

			var parts = expression.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
			switch (parts.Count()) {
				case 1:
					return new SqlResourceIdentity {
						ResourceKey = parts[0]
					};
				case 2:
					return new SqlResourceIdentity {
						ClassKey = parts[0],
						ResourceKey = parts[1]
					};
				default:
					throw new InvalidExpressionException(string.Format(@"""{0}"" is not a valid SqlResourceExpression", expression));
			}
		}
	}
}