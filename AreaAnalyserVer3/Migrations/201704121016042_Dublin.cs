namespace AreaAnalyserVer3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dublin : DbMigration
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
                        Name = c.String(),
                        Point = c.Geography(),
                    })
                .PrimaryKey(t => t.StationId);
            
            CreateTable(
                "dbo.Offence",
                c => new
                    {
                        OffenceId = c.Int(nullable: false, identity: true),
                        StationId = c.Int(nullable: false),
                        TypeOfOffence = c.String(),
                        Year = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        StationAddress = c.String(),
                    })
                .PrimaryKey(t => t.OffenceId)
                .ForeignKey("dbo.GardaStation", t => t.StationId, cascadeDelete: true)
                .Index(t => t.StationId);
            
            CreateTable(
                "dbo.Town",
                c => new
                    {
                        TownId = c.Int(nullable: false, identity: true),
                        GardaId = c.Int(),
                        Name = c.String(),
                        IrishSpelling = c.String(),
                        OtherSpelling = c.String(),
                        PostCode = c.String(),
                        Population = c.Int(),
                        GeoLocation = c.Geography(),
                    })
                .PrimaryKey(t => t.TownId)
                .ForeignKey("dbo.GardaStation", t => t.GardaId)
                .Index(t => t.GardaId);
            
            CreateTable(
                "dbo.PriceRegister",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        TownId = c.Int(),
                        address = c.String(),
                        PostCode = c.String(),
                        price = c.Double(nullable: false),
                        date_of_sale = c.DateTime(nullable: false),
                        not_full_market = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Town", t => t.TownId)
                .Index(t => t.TownId);
            
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
                        GeoLocation = c.Geography(),
                        geocoded = c.String(maxLength: 2, fixedLength: true, unicode: false),
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
                        Level = c.String(maxLength: 5),
                        Type = c.String(),
                        County = c.String(),
                        Address = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Gender = c.String(maxLength: 2, fixedLength: true, unicode: false),
                        AttendanceType = c.String(maxLength: 18),
                        FeePaying = c.Int(),
                        MaleEnrol = c.Int(),
                        FemaleEnrol = c.Int(),
                        Total = c.Int(),
                        Ethos = c.String(),
                        Gaeltacht = c.String(maxLength: 2, fixedLength: true, unicode: false),
                        DEIS = c.String(maxLength: 2, fixedLength: true, unicode: false),
                        Eircode = c.String(maxLength: 8),
                        GeoLocation = c.Geography(),
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
                        GMIT = c.Int(),
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
                        Progressed = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.LeaverId)
                .ForeignKey("dbo.School", t => t.SchoolId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Leaver", "SchoolId", "dbo.School");
            DropForeignKey("dbo.School", "TownId", "dbo.Town");
            DropForeignKey("dbo.Business", "TownId", "dbo.Town");
            DropForeignKey("dbo.PriceRegister", "TownId", "dbo.Town");
            DropForeignKey("dbo.Town", "GardaId", "dbo.GardaStation");
            DropForeignKey("dbo.AnnualReport", "StationId", "dbo.GardaStation");
            DropForeignKey("dbo.Offence", "StationId", "dbo.GardaStation");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Leaver", new[] { "SchoolId" });
            DropIndex("dbo.School", new[] { "TownId" });
            DropIndex("dbo.Business", new[] { "TownId" });
            DropIndex("dbo.PriceRegister", new[] { "TownId" });
            DropIndex("dbo.Town", new[] { "GardaId" });
            DropIndex("dbo.Offence", new[] { "StationId" });
            DropIndex("dbo.AnnualReport", new[] { "StationId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Leaver");
            DropTable("dbo.School");
            DropTable("dbo.Business");
            DropTable("dbo.PriceRegister");
            DropTable("dbo.Town");
            DropTable("dbo.Offence");
            DropTable("dbo.GardaStation");
            DropTable("dbo.AnnualReport");
        }
    }
}
