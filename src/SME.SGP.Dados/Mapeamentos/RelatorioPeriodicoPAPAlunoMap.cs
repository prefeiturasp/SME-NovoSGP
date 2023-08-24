using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioPeriodicoPAPAlunoMap : BaseMap<RelatorioPeriodicoPAPAluno>
    {
        public RelatorioPeriodicoPAPAlunoMap()
        {
            ToTable("relatorio_periodico_pap_aluno");

            Map(c => c.CodigoAluno).ToColumn("aluno_codigo");
            Map(c => c.NomeAluno).ToColumn("aluno_nome");
            Map(c => c.RelatorioPeriodicoTurmaId).ToColumn("relatorio_periodico_pap_turma_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
