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
            apikey = apikey.Trim();
            return string.IsNullOrWhiteSpace(apikey)
                ?  appBuilder.Use<CatifyMiddleware>()
                :  appBuilder.Use<CatifyMiddleware>(apikey);
        }
    }
    
}
