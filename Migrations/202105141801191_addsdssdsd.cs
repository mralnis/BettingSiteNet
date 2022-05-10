namespace BettingSiteNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsdssdsd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournaments", "IsActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.Tournaments", "TieAllowed");

            


        }
        
        public override void Down()
        {
            AddColumn("dbo.Tournaments", "TieAllowed", c => c.Boolean(nullable: false));
            DropColumn("dbo.Tournaments", "IsActive");
        }
    }
}
