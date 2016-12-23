namespace AreaAnalyserVer3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class towns : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Town",
                c => new
                    {
                        TownId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        County = c.String(),
                        GeoLocation = c.Geography(),
                    })
                .PrimaryKey(t => t.TownId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Town");
        }
    }
}
