using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioSemestralPAPAlunoMap : BaseEntityMap<RelatorioSemestralPAPAluno>
    {
        public RelatorioSemestralPAPAlunoMap()
        {
            ToTable("relatorio_semestral_pap_aluno");

            Map(nameof(RelatorioSemestralPAPAluno.RelatorioSemestralTurmaPAPId), "relatorio_semestral_turma_pap_id");
            Map(nameof(RelatorioSemestralPAPAluno.AlunoCodigo), "aluno_codigo");
            Map(nameof(RelatorioSemestralPAPAluno.Migrado), "migrado");
            Map(nameof(RelatorioSemestralPAPAluno.Excluido), "excluido");
        }
    }
}