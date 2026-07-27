using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaFechamentoMap : BaseEntityMap<PendenciaFechamento>
    {
        public PendenciaFechamentoMap()
        {
            ToTable("pendencia_fechamento");
            Map(nameof(PendenciaFechamento.FechamentoTurmaDisciplinaId), "fechamento_turma_disciplina_id");
            Map(nameof(PendenciaFechamento.PendenciaId), "pendencia_id");
        }
    }
}