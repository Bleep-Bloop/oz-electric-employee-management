using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OzElectric_EmployeeManagement.Models;
using System;

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

            if (!RoleManager.RoleExists("Admin"))
            {
                //Create Admin Role
                IdentityRole AdminRole = new IdentityRole();
                AdminRole.Name = "Admin";
                RoleManager.Create(AdminRole);
            }

            if (!RoleManager.RoleExists("Accounting"))
            {
                //Create Accounting Role
                IdentityRole AccountingRole = new IdentityRole();
                AccountingRole.Name = "Accounting";
                RoleManager.Create(AccountingRole);
            }

            if (!RoleManager.RoleExists("Employee"))
            {
                //Create Employee Role
                IdentityRole EmployeeRole = new IdentityRole();
                EmployeeRole.Name = "Employee";
                RoleManager.Create(EmployeeRole);
            }

            if (!RoleManager.RoleExists("Guest"))
            {
                //Create Guest Role
                IdentityRole GuestRole = new IdentityRole();
                GuestRole.Name = "Guest";
                RoleManager.Create(GuestRole);
            }

            if (UserManager.FindByEmail("email@admin.com") == null)
            {
                //create admin user
                ApplicationUser AdminUser = new ApplicationUser();
                AdminUser.Email = "email@admin.com";
                AdminUser.UserName = AdminUser.Email;
                AdminUser.EmailConfirmed = true;
                string adminPassword = "administrator";

                IdentityResult checkAdmin = UserManager.Create(AdminUser, adminPassword);

                //If user was created - add the role to user
                if (checkAdmin.Succeeded)
                {
                    IdentityResult result = UserManager.AddToRole(AdminUser.Id, "Admin");
                }
            }

            if (UserManager.FindByEmail("email@accounting.com") == null)
            {
                //create Accounting user
                ApplicationUser AccountingUser = new ApplicationUser();
                AccountingUser.Email = "email@accounting.com";
                AccountingUser.UserName = AccountingUser.Email;
                AccountingUser.EmailConfirmed = true;
                string AccountingPassword = "accounting";

                IdentityResult checkAccounting = UserManager.Create(AccountingUser, AccountingPassword);

                if (checkAccounting.Succeeded)
                {
                    IdentityResult result = UserManager.AddToRole(AccountingUser.Id, "Accounting");
                }
            }

            if (UserManager.FindByEmail("email@employee.com") == null)
            {
                //create Employee user
                ApplicationUser EmployeeUser = new ApplicationUser();
                EmployeeUser.Email = "email@employee.com";
                EmployeeUser.UserName = EmployeeUser.Email;
                EmployeeUser.EmailConfirmed = true;
                string EmployeePassword = "employee";

                IdentityResult checkEmployee = UserManager.Create(EmployeeUser, EmployeePassword);

                if (checkEmployee.Succeeded)
                {
                    IdentityResult result = UserManager.AddToRole(EmployeeUser.Id, "Employee");
                }
            }

            if (UserManager.FindByEmail("email@guest.com") == null)
            {
                //create Guest user
                ApplicationUser GuestUser = new ApplicationUser();
                GuestUser.Email = "email@guest.com";
                GuestUser.UserName = GuestUser.Email;
                GuestUser.EmailConfirmed = true;
                string GuestPassword = "password";

                IdentityResult checkGuest = UserManager.Create(GuestUser, GuestPassword);

                if (checkGuest.Succeeded)
                {
                    IdentityResult result = UserManager.AddToRole(GuestUser.Id, "Guest");
                }
            }
   
        }
    }
}
