using System;

namespace NancyTest
{
	public class HomeModule : Nancy.NancyModule
	{
		public HomeModule ()
		{
			Get ["/home"] = _ => @"
<html>
	<body>
		<h1>Hello</h1>
		<img src='http://www.webstep.com/wp-content/uploads/2012/01/greatplacetowork2013.jpg'>
	</body>
</html>";
		}
	}
}

