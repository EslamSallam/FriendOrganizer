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
        }
    }
}
