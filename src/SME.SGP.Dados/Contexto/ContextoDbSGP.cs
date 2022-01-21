using Microsoft.EntityFrameworkCore;
using SME.SGP.Dados.Mapeamentos.Entity;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Contexto
{
    public class ContextoDbSGP : DbContext
    {
        public ContextoDbSGP(DbContextOptions<ContextoDbSGP> options)
         : base(options)
        { }

        public DbSet<ComponenteCurricularSgp> ComponentesCurricularesSgp { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);            
            modelBuilder.ApplyConfiguration(new ComponenteCurricularSgpEntityMap());
        }
    }
}