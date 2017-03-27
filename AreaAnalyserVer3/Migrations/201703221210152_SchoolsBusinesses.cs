namespace AreaAnalyserVer3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SchoolsBusinesses : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.arealyser_ppr", newName: "PriceRegister");
            CreateTable(
                "dbo.Business",
                c => new
                    {
                        BusinessId = c.Int(nullable: false, identity: true),
                        TownId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Address = c.String(),
                        Category = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.BusinessId)
                .ForeignKey("dbo.Town", t => t.TownId, cascadeDelete: true)
                .Index(t => t.TownId);
            
            CreateTable(
                "dbo.School",
                c => new
                    {
                        SchoolId = c.Int(nullable: false, identity: true),
                        TownId = c.Int(),
                        Name = c.String(nullable: false),
                        Level = c.String(),
                        County = c.String(),
                        Address = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        FeePaying = c.Int(),
                        MaleEnrol = c.Int(),
                        FemaleEnrol = c.Int(),
                        Ethos = c.String(),
                    })
                .PrimaryKey(t => t.SchoolId)
                .ForeignKey("dbo.Town", t => t.TownId)
                .Index(t => t.TownId);
            
            CreateTable(
                "dbo.Leaver",
                c => new
                    {
                        LeaverId = c.Int(nullable: false, identity: true),
                        SchoolId = c.Int(),
                        Name = c.String(nullable: false),
                        SatLeaving = c.Int(nullable: false),
                        NumAccepted = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        UCD = c.Int(),
                        TCD = c.Int(),
                        DCU = c.Int(),
                        UL = c.Int(),
                        Maynooth = c.Int(),
                        NUIG = c.Int(),
                        UCC = c.Int(),
                        StAngelas = c.Int(),
                        QUB = c.Int(),
                        UU = c.Int(),
                        BlanchIT = c.Int(),
                        NatCol = c.Int(),
                        DIT = c.Int(),
                        ITTD = c.Int(),
                        AthloneIT = c.Int(),
                        Cork = c.Int(),
                        Dundalk = c.Int(),
                        IADT = c.Int(),
                        ITCarlow = c.Int(),
                        ITSligo = c.Int(),
                        ITTralee = c.Int(),
                        ITLetterkenny = c.Int(),
                        ITLimerick = c.Int(),
                        WIT = c.Int(),
                        Marino = c.Int(),
                        CofI = c.Int(),
                        MaryImac = c.Int(),
                        NCAD = c.Int(),
                        RCSI = c.Int(),
                        Shannon = c.Int(),
                        DEIS = c.Int(nullable: false),
                        Progressed = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.LeaverId)
                .ForeignKey("dbo.School", t => t.SchoolId)
                .Index(t => t.SchoolId);
            
            AddColumn("dbo.Town", "GardaId", c => c.Int());
            AddColumn("dbo.Town", "IrishSpelling", c => c.String());
            AddColumn("dbo.Town", "Garda_StationId", c => c.Int());
            AlterColumn("dbo.PriceRegister", "county", c => c.String(maxLength: 16));
            AlterColumn("dbo.Town", "County", c => c.String(maxLength: 16));
            CreateIndex("dbo.Town", "County");
            CreateIndex("dbo.Town", "Garda_StationId");
            CreateIndex("dbo.PriceRegister", "county", name: "IX_County");
            AddForeignKey("dbo.Town", "Garda_StationId", "dbo.GardaStation", "StationId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Leaver", "SchoolId", "dbo.School");
            DropForeignKey("dbo.School", "TownId", "dbo.Town");
            DropForeignKey("dbo.Business", "TownId", "dbo.Town");
            DropForeignKey("dbo.Town", "Garda_StationId", "dbo.GardaStation");
            DropIndex("dbo.PriceRegister", "IX_County");
            DropIndex("dbo.Leaver", new[] { "SchoolId" });
            DropIndex("dbo.School", new[] { "TownId" });
            DropIndex("dbo.Business", new[] { "TownId" });
            DropIndex("dbo.Town", new[] { "Garda_StationId" });
            DropIndex("dbo.Town", new[] { "County" });
            AlterColumn("dbo.Town", "County", c => c.String());
            AlterColumn("dbo.PriceRegister", "county", c => c.String());
            DropColumn("dbo.Town", "Garda_StationId");
            DropColumn("dbo.Town", "IrishSpelling");
            DropColumn("dbo.Town", "GardaId");
            DropTable("dbo.Leaver");
            DropTable("dbo.School");
            DropTable("dbo.Business");
            RenameTable(name: "dbo.PriceRegister", newName: "arealyser_ppr");
        }
    }
}
