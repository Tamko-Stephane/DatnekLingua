using DatnekLingua_API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DatnekLingua_API.Data
{
    public class LanguagesDbContext : DbContext
    {
        public LanguagesDbContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureCreated();
        }

        #region DbSets
        public DbSet<Language> Languages { get; set; }
        public DbSet<UserConfiguredLanguage> UserConfiguredLanguages { get; set; }
        public DbSet<NiveauParle> NiveauxParles { get; set; }
        public DbSet<NiveauEcrit> NiveauxEcrits { get; set; }
        public DbSet<NiveauComprehension> NiveauxComprehensions { get; set; }
        #endregion

        #region Method override

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
        #endregion

    }
}
