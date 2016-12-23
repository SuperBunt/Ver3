namespace AreaAnalyserVer3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class earlyData : DbMigration
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
                "dbo.arealyser_ppr",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        address = c.String(),
                        price = c.Double(nullable: false),
                        date_of_sale = c.DateTime(nullable: false),
                        county = c.String(),
                        not_full_market = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
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
            DropForeignKey("dbo.Offence", "StationId", "dbo.GardaStation");
            DropForeignKey("dbo.AnnualReport", "StationId", "dbo.GardaStation");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Offence", new[] { "StationId" });
            DropIndex("dbo.AnnualReport", new[] { "StationId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.arealyser_ppr");
            DropTable("dbo.Offence");
            DropTable("dbo.GardaStation");
            DropTable("dbo.AnnualReport");
        }
    }
}
