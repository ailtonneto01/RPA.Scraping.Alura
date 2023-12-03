using Microsoft.EntityFrameworkCore;
using RPA.Scraping.Alura.Domain.Entities;
using RPA.Scraping.Alura.Infra.Data.Map;

namespace RPA.Scraping.Alura.Infra.Data.Context
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options) { }

        public DbSet<Curso> Cursos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Curso>(new CursoMap().Configure);
        }
    }
}
