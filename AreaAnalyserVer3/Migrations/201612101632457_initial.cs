namespace AreaAnalyserVer3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PriceRegister", newName: "arealyser_ppr");
            RenameColumn(table: "dbo.arealyser_ppr", name: "PriceRegisterId", newName: "id");
            RenameColumn(table: "dbo.arealyser_ppr", name: "DateOfSale", newName: "date_of_sale");
            AddColumn("dbo.arealyser_ppr", "not_full_market", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.arealyser_ppr", "not_full_market");
            RenameColumn(table: "dbo.arealyser_ppr", name: "date_of_sale", newName: "DateOfSale");
            RenameColumn(table: "dbo.arealyser_ppr", name: "id", newName: "PriceRegisterId");
            RenameTable(name: "dbo.arealyser_ppr", newName: "PriceRegister");
        }
    }
}
