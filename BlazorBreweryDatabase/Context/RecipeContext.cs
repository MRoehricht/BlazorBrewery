﻿using BlazorBrewery.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorBrewery.Database.Context
{
    public class RecipeContext : DbContext
    {

        public DbSet<RecipeEntity> Recipes { get; set; }
        public DbSet<IngredientEntity> Ingredients { get; set; }
        public DbSet<StepEntity> Steps { get; set; }
        public DbSet<UnitEntity> Units { get; set; }
        public DbSet<PumpIntervalEntity> PumpIntervals { get; set; }

        public RecipeContext() : base()
        {
            //PS C:\Users\Matth\source\Git\BlazorBrewery\BlazorBreweryDatabase> dotnet ef migrations add InitialCreate
        }

        public RecipeContext(DbContextOptions<RecipeContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=BlazorBrewery.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IngredientEntity>()
            .HasOne(p => p.Recipe)
            .WithMany(b => b.Ingredients)
            .HasForeignKey(p => p.RecipeId)
            .HasPrincipalKey(b => b.Id);


            modelBuilder.Entity<IngredientEntity>()
            .HasOne(p => p.Unit)
            .WithMany(b => b.Ingredients)
            .HasForeignKey(p => p.UnitId)
            .HasPrincipalKey(b => b.Id);


            modelBuilder.Entity<StepEntity>()
            .HasOne(p => p.Recipe)
            .WithMany(b => b.Steps)
            .HasForeignKey(p => p.RecipeId)
            //.HasForeignKey(p => p.PumpIntervalId)
            .HasPrincipalKey(b => b.Id);


        }
    }
}
