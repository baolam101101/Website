namespace Website.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Website.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Website.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Website.Models.ApplicationDbContext";
        }

        protected override void Seed(Website.Models.ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                CreateDepartment(context);

                var QAEmail = "QA@gmail.com";
                var QAUserName = QAEmail;
                var QAFullName = "QA Manager";
                var QAPassword = "1";
                var QARole = "QA Manager";
                var QADepartment = 1;

                CreateQAUser(context, QAEmail, QAUserName, QAFullName, QAPassword, QARole, QADepartment);
            }
        }

        private void CreateDepartment(ApplicationDbContext context)
        {
            context.Departments.Add(new Department()
            {
                DepartmentId = 1,
                Name = "QA"
            });
        }

        private void CreateQAUser(ApplicationDbContext context, string QAEmail, string QAUserName, string QAFullName, string QAPassword, string QARole, int QADepartment)
        {
            var QAUser = new ApplicationUser()
            {
                UserName = QAUserName,
                FullName = QAFullName,
                Email = QAEmail,
                DepartmentId = QADepartment
            };

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var userCreateResult = userManager.Create(QAUser, QAPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(QARole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }

            var addTrainingstaffRoleResult = userManager.AddToRole(QAUser.Id, QARole);
            if (!addTrainingstaffRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addTrainingstaffRoleResult.Errors));
            }
        }
    }
}
