using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaRegistroIndividualMap : BaseMap<PendenciaRegistroIndividual>
    {
        public PendenciaRegistroIndividualMap()
        {
            ToTable("pendencia_registro_individual");
            Map(x => x.PendenciaId).ToColumn("pendencia_id");
            Map(x => x.TurmaId).ToColumn("turma_id");
        }
    }
}