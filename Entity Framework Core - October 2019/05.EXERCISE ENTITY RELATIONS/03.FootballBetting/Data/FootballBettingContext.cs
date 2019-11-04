namespace P03_FootballBetting.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class FootballBettingContext : DbContext
    {
        //public FootballBettingContext(DbContextOptions options)
        //    : base(options)
        //{

        //}

        public DbSet<Team> Teams { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureTeamEntity(modelBuilder);

            ConfigureColorEntity(modelBuilder);

            ConfigureTownEntity(modelBuilder);

            ConfigureCountryEntity(modelBuilder);

            ConfigurePlayerEntity(modelBuilder);

            ConfigurePositionEntity(modelBuilder);

            ConfigurePlayerStatisticEntity(modelBuilder);

            ConfigureGameEntity(modelBuilder);

            ConfigureBetEntity(modelBuilder);

            ConfigureUserEntity(modelBuilder);
        }

        private void ConfigureUserEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Bets)
                .WithOne(b => b.User);           
        }

        private void ConfigureBetEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bet>()
               .HasKey(g => g.BetId);

            modelBuilder.Entity<Bet>()
                .Property(b => b.Prediction)
                .IsRequired();
        }

        private void ConfigureGameEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasKey(g => g.GameId);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Bets)
                .WithOne(b => b.Game);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.PlayerStatistics)
                .WithOne(ps => ps.Game);
        }

        private void ConfigurePlayerStatisticEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerStatistic>()
                .HasKey(ps => new { ps.PlayerId, ps.GameId });            
        }

        private void ConfigurePositionEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>()
                .HasKey(p => p.PositionId);

            modelBuilder.Entity<Position>()
                .HasMany(p => p.Players)
                .WithOne(p => p.Position);

            modelBuilder.Entity<Position>()
               .Property(c => c.Name)
               .IsRequired();
        }

        private void ConfigurePlayerEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasKey(p => p.PlayerId);

            modelBuilder.Entity<Player>()
                .HasMany(p => p.PlayerStatistics)
                .WithOne(ps => ps.Player);

            modelBuilder.Entity<Player>()
                .Property(p => p.Name)
                .IsRequired();
        }

        private void ConfigureCountryEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .HasKey(c => c.CountryId);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.Towns)
                .WithOne(t => t.Country);

            modelBuilder.Entity<Country>()
                .Property(c => c.Name)
                .IsRequired();
        }

        private void ConfigureTownEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Town>()
                .HasKey(t => t.TownId);

            modelBuilder.Entity<Town>()
                .HasMany(tn => tn.Teams)
                .WithOne(tm => tm.Town);           
        }

        private void ConfigureColorEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Color>()
                .HasKey(c => c.ColorId);

            modelBuilder.Entity<Color>()
                .HasMany(c => c.PrimaryKitTeams)
                .WithOne(pr => pr.PrimaryKitColor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Color>()
                .HasMany(c => c.SecondaryKitTeams)
                .WithOne(s => s.SecondaryKitColor)
                .OnDelete(DeleteBehavior.Restrict);            
        }

        private void ConfigureTeamEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>()
                .HasKey(t => t.TeamId);

            modelBuilder.Entity<Team>()
                .HasMany(t => t.Players)
                .WithOne(p => p.Team);

            modelBuilder.Entity<Team>()
                .HasMany(t => t.HomeGames)
                .WithOne(g => g.HomeTeam)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
               .HasMany(t => t.AwayGames)
               .WithOne(g => g.AwayTeam)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .Property(t => t.Name)
                .IsRequired();
        }
    }
}
