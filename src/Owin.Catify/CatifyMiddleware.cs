using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Owin;
using System;
using System.Collections.Specialized;

namespace Owin.Catify
{
    public class CatifyMiddleware : OwinMiddleware
    {
        private string _apiKey = "";

        public CatifyMiddleware(OwinMiddleware next, string apiKey) : base(next)
        {
            _apiKey = apiKey;
        }

		public CatifyMiddleware(OwinMiddleware next) : this(next, "") { }

        public override Task Invoke(IOwinContext context)
        {
            string[] contentType; 
            if (context.Request.Headers.TryGetValue("content-type", out contentType))
            {
                if (contentType.First() != "text/html")
                    return Next.Invoke(context);
            }

            context.Response.Body = new ImageSourceHighjackerStream(
				GetUrl(), 
				context.Response.Body, 
				Encoding.UTF8);

			return Next.Invoke(context);
        }

        private string GetUrl()
        {
            string url = "http://thecatapi.com/api/images/get";
            var queryString = new NameValueCollection();
            queryString.Add("size", "small");

            if (!string.IsNullOrEmpty(_apiKey))
            {
                queryString.Add("apiKey", _apiKey);
            }
            url = string.Format("{0}?{1}", url, ToQueryString(queryString));
            return url;
        }

        private string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                    from value in nvc.GetValues(key)
                    select string.Format("{0}={1}", System.Uri.EscapeUriString(key), System.Uri.EscapeUriString(value)))
                    .ToArray();
            return "?" + string.Join("&", array);
        }
    }
}
