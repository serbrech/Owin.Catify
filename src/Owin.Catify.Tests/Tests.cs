using System;
using NUnit.Framework;
using System.Net.Http;
using Microsoft.Owin.Testing;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;

namespace Owin.Catify.Tests
{
	[TestFixture]
	public class Tests
	{
		[Test]
		public void replace_imgsrc()
		{
			var client = CreateHttpClient ();
			var response =	client.GetAsync("http://localhost/");
			var result = response.GetAwaiter ().GetResult ();
			result.StatusCode.Should ().Be (HttpStatusCode.OK);
		    var content = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            content.Should().Be("<html><head></head><body><img src=\"http://thecatapi.com/api/images/get?size=small\"></body></html>");
		}

		private HttpClient CreateHttpClient()
		{
			return TestServer.Create(builder => 
				builder
				.Use<CatifyMiddleware>("ABCD")
				.Run(ctx => ctx.Response.WriteAsync("<html><head></head><body><img src=\"../../img/dog.png\"></body></html>"))).HttpClient;
		}
	}
}

