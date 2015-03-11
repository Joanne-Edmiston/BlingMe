namespace BlingMe.Domain.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imagecolumnforbracelets : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bracelet", "Avatar", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bracelet", "Avatar");
        }
    }
}
