using xx0000xWebAppIEP.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;

[assembly: OwinStartupAttribute(typeof(xx0000xWebAppIEP.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace xx0000xWebAppIEP
{
    public partial class Startup
    {
        private static Timer _timer;
        private  TimeSpan _updateInterval = TimeSpan.FromSeconds(30);

        private static ApplicationDbContext db = new ApplicationDbContext();

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
            app.MapSignalR();

            _timer = new Timer(UpdateFinishedAuctions, null, _updateInterval, _updateInterval);

          //  new Thread(UpdateFinishedAuctions).Start();

        }

        private void createRolesandUsers()
        {

            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "admin@admin.com";
                user.Email = "admin@admin.com";
                user.FName = "Admin";
                user.LName = "Admin";
                user.NoTokens = 0;

                string userPWD = "ASD123!asd";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

        }

        private static void UpdateFinishedAuctions(object state)
        {
            

            DateTime currentTime = DateTime.Now.ToUniversalTime();
            List<Auction> exAuctions = db.Auctions.Where(a => a.Status == 2).Where(a => a.ClosingDT < currentTime).ToList();

            foreach (Auction auction in exAuctions)
            {
                DateTime closing = (DateTime)auction.ClosingDT;

                if (closing.AddSeconds(11) < currentTime)
                {
                    //sold or exp
                    if (auction.AspNetUserId != null)
                    {
                        //sold
                        auction.Status = 3;
                    }
                    else
                    {
                        //exp
                        auction.Status = 4;

                    }

                    byte[] rowVersion = auction.RowVersion;

                    try
                    {
                        db.Entry(auction).OriginalValues["RowVersion"] = rowVersion;
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        //nista, idemo dalje
                    }


                }

            }


        }
    }
}
