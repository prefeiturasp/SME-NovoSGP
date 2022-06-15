using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaRegistroIndividualMap : BaseMap<PendenciaRegistroIndividual>
    {
        public PendenciaRegistroIndividualMap()
        {
            ToTable("pendencia_registro_individual");
            Map(c => c.PendenciaId).ToColumn("pendencia_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
        }
    }
}