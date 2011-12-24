using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Core.MVC.Localization
{
	public class SqlResourceProviderSection : ConfigurationSection
	{
		public SqlResourceProviderSection() { }
		public SqlResourceProviderSection(string connectionStringName)
		{
			ConnectionStringName = connectionStringName;
		}

		[ConfigurationProperty("connectionStringName"), Description("The name of the connectionstring (configured in the connectionString section) used to connect to the database")]
		public string ConnectionStringName
		{
			get { return this["connectionStringName"] as string; }
			set { this["connectionStringName"] = value; }
		}

		[ConfigurationProperty("className")]
		public string ClassName
		{
			get { return this["className"] as string; }
			set { this["className"] = value; }
		}

		[ConfigurationProperty("defaultLanguage")]
		public string DefaultLanguage
		{
			get { return this["defaultLanguage"] as string; }
			set { this["defaultLanguage"] = value; }
		}

		[ConfigurationProperty("project")]
		public string Project
		{
			get { return this["project"] as string; }
			set { this["project"] = value; }
		}

		[ConfigurationProperty("table")]
		public string Table
		{
			get { return this["table"] as string; }
			set { this["table"] = value; }
		}

		[ConfigurationProperty("tableNamespace")]
		public string TableNamespace
		{
			get { return this["tableNamespace"] as string; }
			set { this["tableNamespace"] = value; }
		}

		[ConfigurationProperty("projectColumn")]
		public string ProjectColumn
		{
			get { return this["projectColumn"] as string; }
			set { this["projectColumn"] = value; }
		}

		[ConfigurationProperty("namespaceColumn")]
		public string NamespaceColumn
		{
			get { return this["namespaceColumn"] as string; }
			set { this["namespaceColumn"] = value; }
		}

		[ConfigurationProperty("classColumn")]
		public string ClassColumn
		{
			get { return this["classColumn"] as string; }
			set { this["classColumn"] = value; }
		}

		[ConfigurationProperty("keyColumn")]
		public string KeyColumn
		{
			get { return this["keyColumn"] as string; }
			set { this["keyColumn"] = value; }
		}

		[ConfigurationProperty("languageColumn")]
		public string LanguageColumn
		{
			get { return this["languageColumn"] as string; }
			set { this["languageColumn"] = value; }
		}

		[ConfigurationProperty("valueColumn")]
		public string ValueColumn
		{
			get { return this["valueColumn"] as string; }
			set { this["valueColumn"] = value; }
		}
	}
}