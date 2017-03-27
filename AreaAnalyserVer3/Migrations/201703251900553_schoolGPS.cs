namespace AreaAnalyserVer3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class schoolGPS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.School", "Type", c => c.String());
            AddColumn("dbo.School", "Total", c => c.Int());
            AddColumn("dbo.School", "Eircode", c => c.String(maxLength: 8));
            AddColumn("dbo.School", "GeoLocation", c => c.Geography());
            DropColumn("dbo.School", "Level");
        }
        
        public override void Down()
        {
            AddColumn("dbo.School", "Level", c => c.String());
            DropColumn("dbo.School", "GeoLocation");
            DropColumn("dbo.School", "Eircode");
            DropColumn("dbo.School", "Total");
            DropColumn("dbo.School", "Type");
        }
    }
}
