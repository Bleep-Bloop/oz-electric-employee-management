using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OzElectric_EmployeeManagement.Models;

[assembly: OwinStartupAttribute(typeof(OzElectric_EmployeeManagement.Startup))]
namespace OzElectric_EmployeeManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            //Define DB Context
            ApplicationDbContext context = new ApplicationDbContext();

            //Create Role Manager / User Manager for creating user and roles
            RoleManager<IdentityRole> RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (RoleManager.RoleExists("Manager"))
            {
                IdentityRole Role = RoleManager.FindByName("Manager");
                RoleManager.Delete(Role);
            }

            //If admin role doesn't exist Create it
            if (!RoleManager.RoleExists("Manager"))
            {
                IdentityRole Role = new IdentityRole();
                Role.Name = "Manager";
                RoleManager.Create(Role);

                var oldUser = UserManager.FindByEmail("OzzEmployeeManager@outlook.com");

                if(oldUser != null)
                {
                    UserManager.Delete(oldUser);
                }

                //create admin user
                ApplicationUser user = new ApplicationUser();
                user.UserName = "OzzEmployeeManager@outlook.com";
                user.Email = "OzzEmployeeManager@outlook.com";
                user.EmailConfirmed = true;

                string userPassword = "29wosn45";

                IdentityResult checkUser = UserManager.Create(user, userPassword);

                //If user was created - add the admin role to user
                if (checkUser.Succeeded)
                {
                    IdentityResult result = UserManager.AddToRole(user.Id, "Manager");
                }
            }

        }
    }
}
