using BettingSiteNet.Migrations;
using BettingSiteNet.Models;
using Microsoft.Owin;
using Owin;
using System.Data.Entity.Migrations;

[assembly: OwinStartupAttribute(typeof(BettingSiteNet.Startup))]
namespace BettingSiteNet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            var context = new ApplicationDbContext();
            //context.Database.Delete();
            context.Database.CreateIfNotExists();

            var migrator = new DbMigrator(new Configuration());
            migrator.Update();
        }
    }
}
