using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(V1Requestor.Startup))]
namespace V1Requestor
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            
        }
    }
}
