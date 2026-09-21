using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioPeriodicoPAPAlunoMap : BaseMap<RelatorioPeriodicoPAPAluno>
    {
        public RelatorioPeriodicoPAPAlunoMap()
        {
            ToTable("relatorio_periodico_pap_aluno");

            Map(nameof(RelatorioPeriodicoPAPAluno.CodigoAluno), "aluno_codigo");
            Map(nameof(RelatorioPeriodicoPAPAluno.NomeAluno), "aluno_nome");
            Map(nameof(RelatorioPeriodicoPAPAluno.RelatorioPeriodicoTurmaId), "relatorio_periodico_pap_turma_id");
            Map(nameof(RelatorioPeriodicoPAPAluno.Excluido), "excluido");
        }


    }

}

