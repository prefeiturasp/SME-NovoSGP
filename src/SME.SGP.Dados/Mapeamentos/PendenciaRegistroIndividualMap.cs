using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaRegistroIndividualMap : BaseEntityMap<PendenciaRegistroIndividual>
    {
        public PendenciaRegistroIndividualMap()
        {
            ToTable("pendencia_registro_individual");
            Map(nameof(PendenciaRegistroIndividual.PendenciaId), "pendencia_id");
            Map(nameof(PendenciaRegistroIndividual.TurmaId), "turma_id");
        }
    }
}