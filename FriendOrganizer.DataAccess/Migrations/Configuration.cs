namespace FriendOrganizer.DataAccess.Migrations
{
    using FriendOrganizer.Model;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendOrganizer.DataAccess.FriendOrganizerDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendOrganizer.DataAccess.FriendOrganizerDBContext context)
        {

            // Deletes all data, from all tables, except for __MigrationHistory
            context.Database.ExecuteSqlCommand("sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            context.Database.ExecuteSqlCommand("sp_MSForEachTable 'IF OBJECT_ID(''?'') NOT IN (ISNULL(OBJECT_ID(''[dbo].[__MigrationHistory]''),0)) DELETE FROM ?'");
            context.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");

            context.Friends.AddOrUpdate(
                new Friend { FirstName = "Eslam", LastName = "Hussien" },
                new Friend { FirstName = "Ahmed", LastName = "Hamed" },
                new Friend { FirstName = "Maged", LastName = "Ahmed" },
                new Friend { FirstName = "Hazem", LastName = "Khaled" }
                );
            context.ProgrammingLanguages.AddOrUpdate(
                new ProgrammingLanguage { Name = "C#" },
                new ProgrammingLanguage { Name = "TypeScript" },
                new ProgrammingLanguage { Name = "VB.net" },
                new ProgrammingLanguage { Name = "Swift" },
                new ProgrammingLanguage { Name = "Java" }
                );

            context.SaveChanges();

            context.FriendPhoneNumber.AddOrUpdate(
                new FriendPhoneNumber { Number = "+20 115087345", FriendId = context.Friends.First().Id }
                );

            context.FriendProgrammingLanguage.AddOrUpdate(
                new FriendProgrammingLanguage { FriendId = context.Friends.First().Id, ProgrammingLanguageId = context.ProgrammingLanguages.First().Id }
                );

            context.Meetings.AddOrUpdate(
                new Meeting
                {
                    Title = "C# course",
                    DateFrom = new DateTime(2016, 1, 1),
                    DateTo = new DateTime(2016, 1, 1),
                    Friends = new List<Friend>
                    {
                        context.Friends.Single(f => f.FirstName == "Ahmed" && f.LastName == "Hamed"),
                        context.Friends.Single(f => f.FirstName == "Maged" && f.LastName == "Ahmed")
                    }
                });
        }
    }
}
