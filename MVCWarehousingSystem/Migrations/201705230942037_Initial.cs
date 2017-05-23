namespace MVCWarehousingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockItems",
                c => new
                    {
                        ArticleNumber = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Double(nullable: false),
                        ShelfPosition = c.String(),
                        Quantity = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ArticleNumber);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StockItems");
        }
    }
}
