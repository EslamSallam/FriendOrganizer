namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFriendsMenyProgrammingLanguageskf : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FriendProgrammingLanguage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FriendId = c.Int(nullable: false),
                        ProgrammingLanguageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Friend", t => t.FriendId, cascadeDelete: true)
                .ForeignKey("dbo.ProgrammingLanguage", t => t.ProgrammingLanguageId, cascadeDelete: true)
                .Index(t => t.FriendId)
                .Index(t => t.ProgrammingLanguageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FriendProgrammingLanguage", "ProgrammingLanguageId", "dbo.ProgrammingLanguage");
            DropForeignKey("dbo.FriendProgrammingLanguage", "FriendId", "dbo.Friend");
            DropIndex("dbo.FriendProgrammingLanguage", new[] { "ProgrammingLanguageId" });
            DropIndex("dbo.FriendProgrammingLanguage", new[] { "FriendId" });
            DropTable("dbo.FriendProgrammingLanguage");
        }
    }
}
