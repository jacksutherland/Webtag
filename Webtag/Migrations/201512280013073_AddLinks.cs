namespace Webtag.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLinks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Link",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserProfileId = c.Int(nullable: false),
                        Text = c.String(),
                        Href = c.String(),
                        Order = c.Int(nullable: false),
                        LinkFolderId = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LinkFolder", t => t.LinkFolderId)
                .Index(t => t.LinkFolderId)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.LinkFolder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserProfileId = c.Int(nullable: false),
                        Text = c.String(),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserProfileId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Link", new[] { "UserProfileId" });
            DropIndex("dbo.LinkFolder", new[] { "UserProfileId" });
            DropIndex("dbo.Link", new[] { "LinkFolderId" });
            DropForeignKey("dbo.Link", "LinkFolderId", "dbo.LinkFolder");
            DropTable("dbo.LinkFolder");
            DropTable("dbo.Link");
        }
    }
}
