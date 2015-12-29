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
                        IsParent = c.Boolean(nullable: false),
                        LinkParentId = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Link");
        }
    }
}
