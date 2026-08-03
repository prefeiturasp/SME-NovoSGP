using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConselhoClasseAlunoMap : BaseMap<ConselhoClasseAluno>
    {
        public ConselhoClasseAlunoMap()
        {
            ToTable("conselho_classe_aluno");
            Map(nameof(ConselhoClasseAluno.ConselhoClasseId), "conselho_classe_id");
            Map(nameof(ConselhoClasseAluno.AlunoCodigo), "aluno_codigo");
            Map(nameof(ConselhoClasseAluno.RecomendacoesAluno), "recomendacoes_aluno");
            Map(nameof(ConselhoClasseAluno.RecomendacoesFamilia), "recomendacoes_familia");
            Map(nameof(ConselhoClasseAluno.AnotacoesPedagogicas), "anotacoes_pedagogicas");
            Map(nameof(ConselhoClasseAluno.ConselhoClasseParecerId), "conselho_classe_parecer_id");
            Map(nameof(ConselhoClasseAluno.Excluido), "excluido");
            Map(nameof(ConselhoClasseAluno.Migrado), "migrado");
            Map(nameof(ConselhoClasseAluno.ParecerAlteradoManual), "parecer_alterado_manual");
        }
    }
}