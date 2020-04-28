using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioSemestralAlunoMap: BaseMap<RelatorioSemestralAluno>
    {
        public RelatorioSemestralAlunoMap()
        {
            ToTable("relatorio_semestral_aluno");
            Map(c => c.RelatorioSemestralId).ToColumn("relatorio_semestral_id");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
        }
    }
}
