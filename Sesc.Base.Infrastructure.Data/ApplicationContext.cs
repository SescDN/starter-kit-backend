using Microsoft.EntityFrameworkCore;
using Sesc.Base.Domain.Entities;
using Sesc.Base.Infrastructure.Data.Mappings;

namespace Sesc.Base.Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        { }

        public DbSet<Aluno> Alunos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Aluno>(new AlunoMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
