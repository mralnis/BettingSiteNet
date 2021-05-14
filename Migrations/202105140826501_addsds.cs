namespace BettingSiteNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Predictions", "PointsEarned", c => c.Int());
            AlterColumn("dbo.Predictions", "HomeTeamScore", c => c.Int());
            AlterColumn("dbo.Predictions", "EnemyTeamScore", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Predictions", "EnemyTeamScore", c => c.Int(nullable: false));
            AlterColumn("dbo.Predictions", "HomeTeamScore", c => c.Int(nullable: false));
            DropColumn("dbo.Predictions", "PointsEarned");
        }
    }
}
