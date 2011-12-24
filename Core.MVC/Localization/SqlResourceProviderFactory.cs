using System;
using System.Web.Compilation;

namespace Core.MVC.Localization
{
	public class SqlResourceProviderFactory : ResourceProviderFactory
	{
		public override IResourceProvider CreateGlobalResourceProvider(string className)
		{
			return new SqlResourceProvider(className);
		}

		public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
		{
			throw new NotSupportedException("Local resources are not (yet) supported");
		}
	}
}