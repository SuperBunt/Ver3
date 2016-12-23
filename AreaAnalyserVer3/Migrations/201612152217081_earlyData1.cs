namespace AreaAnalyserVer3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class earlyData1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GardaStation", "Point", c => c.Geography());
            DropColumn("dbo.GardaStation", "Latitiude");
            DropColumn("dbo.GardaStation", "Longitude");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GardaStation", "Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.GardaStation", "Latitiude", c => c.Double(nullable: false));
            DropColumn("dbo.GardaStation", "Point");
        }
    }
}
