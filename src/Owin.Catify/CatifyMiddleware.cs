using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Owin;

namespace Owin.Catify
{
    public static class CatifyExtension
    {
        public static IAppBuilder UseCatify(this IAppBuilder appBuilder, string apikey = null)
        {
            return string.IsNullOrWhiteSpace(apikey)
                ? appBuilder.Use<CatifyMiddleware>(apikey)
                : appBuilder.Use<CatifyMiddleware>();
        }
    }

    public class CatifyMiddleware : OwinMiddleware
    {
        public CatifyMiddleware(OwinMiddleware next)
            : base(next)
        {

        }

        public override Task Invoke(IOwinContext context)
        {
            string[] contentType; 
            if (context.Request.Headers.TryGetValue("content-type", out contentType))
            {
                if (contentType.First() != "text/html")
                    return Next.Invoke(context);
            }
            context.Response.Body = new ImgSrcHighjackerStream("http://thecatapi.com/api/images/get?size=small", context.Response.Body, Encoding.UTF8);
            return Next.Invoke(context);
        }
    }
}
