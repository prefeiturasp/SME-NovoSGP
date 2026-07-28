using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaMap : BaseEntityMap<RecuperacaoParalela>
    {
        public RecuperacaoParalelaMap()
        {
            ToTable("recuperacao_paralela");
            Map(nameof(RecuperacaoParalela.Aluno_id), "aluno_id");
            Map(nameof(RecuperacaoParalela.Excluido), "excluido");
            Map(nameof(RecuperacaoParalela.TurmaId), "turma_id");
            Map(nameof(RecuperacaoParalela.TurmaRecuperacaoParalelaId), "turma_recuperacao_paralela_id");
            Map(nameof(RecuperacaoParalela.AnoLetivo), "ano_letivo");
        }
    }
}