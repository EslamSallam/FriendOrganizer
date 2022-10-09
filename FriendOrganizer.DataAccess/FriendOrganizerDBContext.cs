﻿using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FriendOrganizer.DataAccess
{
    public class FriendOrganizerDBContext : DbContext
    {
        public DbSet<Friend>? Friends { get; set; }
        public FriendOrganizerDBContext() : base("name=DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //Fluent API
            modelBuilder.Entity<Friend>().Property(f => f.LastName).HasMaxLength(50);
            //Fluent API Configuration To Friend Class
            modelBuilder.Configurations.Add(new FriendConfiguration());
        }
    }

    // Adding Class For Friend Configuration
    public class FriendConfiguration : EntityTypeConfiguration<Friend>
    {
        public FriendConfiguration()
        {
            Property(f => f.FirstName).IsRequired().HasMaxLength(50);
        }
    }
}
