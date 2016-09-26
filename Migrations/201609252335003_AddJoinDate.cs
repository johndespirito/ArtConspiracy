namespace ArtConspiracy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJoinDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "joinDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "joinDate");
        }
    }
}
