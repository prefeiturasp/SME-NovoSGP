using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioSemestralPAPAlunoMap: BaseMap<RelatorioSemestralPAPAluno>
    {
        public RelatorioSemestralPAPAlunoMap()
        {
            ToTable("relatorio_semestral_pap_aluno");
            Map(c => c.RelatorioSemestralTurmaPAPId).ToColumn("relatorio_semestral_turma_pap_id");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.Migrado).ToColumn("migrado");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
