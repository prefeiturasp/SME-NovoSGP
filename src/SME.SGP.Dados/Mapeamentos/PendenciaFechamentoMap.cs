using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaFechamentoMap : BaseMap<PendenciaFechamento>
    {
        public PendenciaFechamentoMap()
        {
            ToTable("pendencia_fechamento");
            Map(c => c.FechamentoTurmaDisciplinaId).ToColumn("fechamento_turma_disciplina_id");
            Map(c => c.PendenciaId).ToColumn("pendencia_id");
        }
    }
}
