namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateFriendProgrammingLanguagesRS : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProgrammingLanguageFriend", "ProgrammingLanguage_Id", "dbo.ProgrammingLanguage");
            DropForeignKey("dbo.ProgrammingLanguageFriend", "Friend_Id", "dbo.Friend");
            DropIndex("dbo.ProgrammingLanguageFriend", new[] { "ProgrammingLanguage_Id" });
            DropIndex("dbo.ProgrammingLanguageFriend", new[] { "Friend_Id" });
            DropTable("dbo.ProgrammingLanguageFriend");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProgrammingLanguageFriend",
                c => new
                    {
                        ProgrammingLanguage_Id = c.Int(nullable: false),
                        Friend_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProgrammingLanguage_Id, t.Friend_Id });
            
            CreateIndex("dbo.ProgrammingLanguageFriend", "Friend_Id");
            CreateIndex("dbo.ProgrammingLanguageFriend", "ProgrammingLanguage_Id");
            AddForeignKey("dbo.ProgrammingLanguageFriend", "Friend_Id", "dbo.Friend", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProgrammingLanguageFriend", "ProgrammingLanguage_Id", "dbo.ProgrammingLanguage", "Id", cascadeDelete: true);
        }
    }
}
