namespace BettingSiteNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedCountries : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO COUNTRIES VALUES('Latvija','LV')
                INSERT INTO COUNTRIES VALUES('ASV','US')
                INSERT INTO COUNTRIES VALUES('Krievija','RU')
                INSERT INTO COUNTRIES VALUES('Zviedrija','SE')
                INSERT INTO COUNTRIES VALUES('Kanāda','CA')
                INSERT INTO COUNTRIES VALUES('Kazahstāna','KZ')
                INSERT INTO COUNTRIES VALUES('Itālija','IT')
                INSERT INTO COUNTRIES VALUES('Norvēģija','NO')
                INSERT INTO COUNTRIES VALUES('Somija','FI')
                INSERT INTO COUNTRIES VALUES('Vācija','DE')
                INSERT INTO COUNTRIES VALUES('Čehija','CZ')
                INSERT INTO COUNTRIES VALUES('Lielbritānija','GB')
                INSERT INTO COUNTRIES VALUES('Austrija','AT')
                INSERT INTO COUNTRIES VALUES('Šveice','CH')
            ");
        }
        
        public override void Down()
        {
        }
    }
}
