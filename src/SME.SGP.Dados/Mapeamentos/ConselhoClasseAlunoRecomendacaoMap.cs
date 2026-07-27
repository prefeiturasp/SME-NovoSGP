using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class ConselhoClasseAlunoRecomendacaoMap : SimpleEntityMap<ConselhoClasseAlunoRecomendacao>
    {
        public ConselhoClasseAlunoRecomendacaoMap()
        {
            ToTable("conselho_classe_aluno_recomendacao");
            Map(nameof(ConselhoClasseAlunoRecomendacao.ConselhoClasseAlunoId), "conselho_classe_aluno_id");
            Map(nameof(ConselhoClasseAlunoRecomendacao.ConselhoClasseRecomendacaoId), "conselho_classe_recomendacao_id");
        }
    }
}
