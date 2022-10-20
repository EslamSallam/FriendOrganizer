namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFriendsMenyProgrammingLanguages : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Friend", "FavoriteLanguageId", "dbo.ProgrammingLanguage");
            DropIndex("dbo.Friend", new[] { "FavoriteLanguageId" });
            CreateTable(
                "dbo.ProgrammingLanguageFriend",
                c => new
                    {
                        ProgrammingLanguage_Id = c.Int(nullable: false),
                        Friend_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProgrammingLanguage_Id, t.Friend_Id })
                .ForeignKey("dbo.ProgrammingLanguage", t => t.ProgrammingLanguage_Id, cascadeDelete: true)
                .ForeignKey("dbo.Friend", t => t.Friend_Id, cascadeDelete: true)
                .Index(t => t.ProgrammingLanguage_Id)
                .Index(t => t.Friend_Id);
            
            DropColumn("dbo.Friend", "FavoriteLanguageId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Friend", "FavoriteLanguageId", c => c.Int());
            DropForeignKey("dbo.ProgrammingLanguageFriend", "Friend_Id", "dbo.Friend");
            DropForeignKey("dbo.ProgrammingLanguageFriend", "ProgrammingLanguage_Id", "dbo.ProgrammingLanguage");
            DropIndex("dbo.ProgrammingLanguageFriend", new[] { "Friend_Id" });
            DropIndex("dbo.ProgrammingLanguageFriend", new[] { "ProgrammingLanguage_Id" });
            DropTable("dbo.ProgrammingLanguageFriend");
            CreateIndex("dbo.Friend", "FavoriteLanguageId");
            AddForeignKey("dbo.Friend", "FavoriteLanguageId", "dbo.ProgrammingLanguage", "Id");
        }
    }
}
