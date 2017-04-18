using AreaAnalyserVer3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;


//[assembly: OwinStartupAttribute(typeof(AreaAnalyserVer3.Startup))]
namespace AreaAnalyserVer3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Configuration(app);
            ConfigureAuth(app);
            // createRolesandUsers();  Initialised default user and Administrator
        }

        // In this method we will create default User roles and Admin user for login   
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin role  
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "Pete";
                user.Email = "javabraybowl@gmail.com";

                string userPWD = "ThisIsNotIT";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating User role    
            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

                //Here we create a default user who for testing                  

                var user = new ApplicationUser();
                user.UserName = "Tester";
                user.Email = "peterconnell@outlook.com";

                string userPWD = "LetMeTest99";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role User   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "User");

                }

            }

        }
        }
    }

