using Microsoft.EntityFrameworkCore;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Contexto
{
    public class ContextoDbSGP : DbContext
    {
        public ContextoDbSGP(DbContextOptions<ContextoDbSGP> options)
         : base(options)
        { }

        public DbSet<ComponenteCurricular> ComponentesCurriculares { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ComponenteCurricular>().
            //modelBuilder.ApplyConfiguration(new ComponenteCurricularEntityMap());
        }
    }
}