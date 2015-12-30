namespace Webtag.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWidgets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Widget",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DashboardId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        ObjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .Index(t => t.DashboardId);
            
            AddColumn("dbo.Dashboard", "UserProfileId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropIndex("dbo.Widget", new[] { "DashboardId" });
            DropForeignKey("dbo.Widget", "DashboardId", "dbo.Dashboard");
            DropColumn("dbo.Dashboard", "UserProfileId");
            DropTable("dbo.Widget");
        }
    }
}
