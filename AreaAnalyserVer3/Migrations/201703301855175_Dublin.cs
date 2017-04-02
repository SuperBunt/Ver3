namespace AreaAnalyserVer3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class Dublin : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Town", new[] { "County" });
            DropIndex("dbo.PriceRegister", "IX_County");
            DropColumn("dbo.Town", "GardaId");
            RenameColumn(table: "dbo.Town", name: "Garda_StationId", newName: "GardaId");
            RenameIndex(table: "dbo.Town", name: "IX_Garda_StationId", newName: "IX_GardaId");
            AddColumn("dbo.GardaStation", "Name", c => c.String());
            AddColumn("dbo.Town", "OtherSpelling", c => c.String());
            AddColumn("dbo.Town", "PostCode", c => c.String());
            AddColumn("dbo.Town", "Population", c => c.Int());
            AddColumn("dbo.Business", "GeoLocation", c => c.Geography());
            AddColumn("dbo.School", "Level", c => c.String(maxLength: 5));
            AddColumn("dbo.Leaver", "GMIT", c => c.Int());
            AddColumn("dbo.PriceRegister", "TownId", c => c.Int());
            AddColumn("dbo.PriceRegister", "PostCode", c => c.String());
            AlterColumn("dbo.PriceRegister", "not_full_market", c => c.Int());
            CreateIndex("dbo.PriceRegister", "TownId");
            AddForeignKey("dbo.PriceRegister", "TownId", "dbo.Town", "TownId");
            DropColumn("dbo.GardaStation", "Address");
            DropColumn("dbo.Town", "County");
            DropColumn("dbo.Leaver", "DEIS");
            DropColumn("dbo.PriceRegister", "county");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PriceRegister", "county", c => c.String(maxLength: 16));
            AddColumn("dbo.Leaver", "DEIS", c => c.Int(nullable: false));
            AddColumn("dbo.Town", "County", c => c.String(maxLength: 16));
            AddColumn("dbo.GardaStation", "Address", c => c.String());
            DropForeignKey("dbo.PriceRegister", "TownId", "dbo.Town");
            DropIndex("dbo.PriceRegister", new[] { "TownId" });
            AlterColumn("dbo.PriceRegister", "not_full_market", c => c.Int(nullable: false));
            DropColumn("dbo.PriceRegister", "PostCode");
            DropColumn("dbo.PriceRegister", "TownId");
            DropColumn("dbo.Leaver", "GMIT");
            DropColumn("dbo.School", "Level");
            DropColumn("dbo.Business", "GeoLocation");
            DropColumn("dbo.Town", "Population");
            DropColumn("dbo.Town", "PostCode");
            DropColumn("dbo.Town", "OtherSpelling");
            DropColumn("dbo.GardaStation", "Name");
            RenameIndex(table: "dbo.Town", name: "IX_GardaId", newName: "IX_Garda_StationId");
            RenameColumn(table: "dbo.Town", name: "GardaId", newName: "Garda_StationId");
            AddColumn("dbo.Town", "GardaId", c => c.Int());
            CreateIndex("dbo.PriceRegister", "county", name: "IX_County");
            CreateIndex("dbo.Town", "County");
        }
    }
}
