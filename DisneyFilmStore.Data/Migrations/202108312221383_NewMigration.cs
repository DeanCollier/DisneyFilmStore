namespace DisneyFilmStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Film", "OwnerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Film", "OwnerId", c => c.Guid(nullable: false));
        }
    }
}
