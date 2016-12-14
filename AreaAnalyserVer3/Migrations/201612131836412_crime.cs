namespace AreaAnalyserVer3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class crime : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnnualReport",
                c => new
                    {
                        ReportID = c.Int(nullable: false, identity: true),
                        StationId = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        NumAttemptedMurderAssault = c.Int(nullable: false),
                        NumDangerousActs = c.Int(nullable: false),
                        NumKidnapping = c.Int(nullable: false),
                        NumRobbery = c.Int(nullable: false),
                        NumBurglary = c.Int(nullable: false),
                        NumTheft = c.Int(nullable: false),
                        NumFraud = c.Int(nullable: false),
                        NumDrugs = c.Int(nullable: false),
                        NumWeapons = c.Int(nullable: false),
                        NumDamage = c.Int(nullable: false),
                        NumPublicOrder = c.Int(nullable: false),
                        NumGovernment = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReportID)
                .ForeignKey("dbo.GardaStation", t => t.StationId, cascadeDelete: true)
                .Index(t => t.StationId);
            
            CreateTable(
                "dbo.GardaStation",
                c => new
                    {
                        StationId = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        Latitiude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.StationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnnualReport", "StationId", "dbo.GardaStation");
            DropIndex("dbo.AnnualReport", new[] { "StationId" });
            DropTable("dbo.GardaStation");
            DropTable("dbo.AnnualReport");
        }
    }
}
