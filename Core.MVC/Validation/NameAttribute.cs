using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;

namespace Core.MVC.Validation
{
	public class NameAttribute : DisplayNameAttribute
	{
		public string Normal
		{
			get { return DisplayName; }
			set { DisplayNameValue = value; }
		}

		public string Localized
		{
			get { return DisplayName; }
			set { DisplayNameValue = Localization.Extensions.Resource(new HttpContextWrapper(HttpContext.Current), value); }
		}
	}
}