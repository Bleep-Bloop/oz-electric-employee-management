using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OzElectric_EmployeeManagement.Startup))]
namespace OzElectric_EmployeeManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
